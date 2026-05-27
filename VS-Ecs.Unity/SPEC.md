# SPEC

## §G GOAL
Bubble-shooter (VS): player fires colored bubbles at hex grid; match ≥N same-color → pop group + drop unattached. Session ends: timer expires | board fills | board clears.

## §C CONSTRAINTS
- Unity 2D, orthographic camera
- DragonECS (DCFApixels) — ∀ simulation logic in ECS systems
- VContainer — DI; systems resolved Transient
- UniTask — async animations
- LitMotion — tweens (⊥ DOTween)
- R3 — reactive bindings for MVVM UI layer
- `#if ENABLE_IL2CPP` guards on `EcsAspect` inner classes
- `COMPANYNAME_PROD` define → strips debug logs from build
- UniText (LightSide) — text rendering (⊥ TMPro, ⊥ UnityEngine.UI.Text)
- ⊥ automated tests

## §I INTERFACES
- input: `IInputService.OnEndDrag(Vector2)` → shot trigger
- grid: `IGridParamsService.Params` → `GridParams` (CellSize, Rows, Columns, StartOffset, StartIsEven, VisibleRows)
- level: `ILevel.GridSpawnRoot` — spawn parent transform
- scene: `ICoreGameSceneRefs.GameCamera`; `ISceneService.SetSceneEnabled(int sceneIndex, bool enabled)` — calls `SetActive(enabled)` on ∀ root GOs in loaded scene; no-op if scene not loaded
- pipeline: `CoreFlow` registers ∀ ECS systems; drives `EcsPipeline.Run()` each `Tick()`
- cfg.gameplay: `GameplayRulesConfig.BubblesToPop` (int ≥2, default 3) + `PopScoreBase` (int) + `PopScoreIncrement` (int) + `DropScorePerBubble` (int) + `TimeBonusA` (float) + `TimeBonusB` (float) + `TimeBonusC` (float) + `BoardClearBonus` (int)
- session: `ISessionDataService` — read-only snapshot surface; `Score: ReadOnlyReactiveProperty<int>`; `IsGameEnded: bool`; `Snapshot()` pulls from all registered sources (currently `IScoreService.Total`) + sets `IsGameEnded=true`; `Reset()` clears all; no external direct writes (V19)
- score: `IScoreService` — accumulator; `Add(int delta)` during gameplay; `ComputeFinal(EGameEndReason, int timeRemaining)` → applies bonuses, stores in `Total`; `int Total` readable after `ComputeFinal`; `Reset()` on session start
- ui: `IUISceneReferences` — `Canvas`, `ScreensRoot`, `PopupsRoot`, `MessagesRoot`; impl: `CoreGameSceneRefs` (Gameplay) + `MetaGameSceneReferences` (Meta); injected into `BaseViewService` subclasses as instantiation parent source
     `IPopupService` — `Show<TView>()`, `Hide<TView>()`, `HideAll()`; TView : Popup; VContainer injects ViewModel at instantiation (⊥ caller); `Show` while open → no-op
     `IScreenService` — `Show<TView>()`, `Hide<TView>()`; TView : Screen; ≤1 active at a time; tutorial overlays ⊥ this service
     `BaseViewService` — abstract; shared mechanics: resolve prefab via `IViewSourceProvider`, instantiate via `IObjectResolver` under `IUISceneReferences` parent, cache per type, drive open/close lifecycle
     `ScreenService : BaseViewService` — injects `IUISceneReferences`; uses `ScreensRoot` as parent; `HideAll()` before each `Show` (V27)
     `IViewSourceProvider` — `GetPrefab<TView>() : BaseView`; impl: `ViewSourceConfig : ScriptableObject` (prefab refs); future: bundle-backed impl
     `BaseView : MonoBehaviour` — open/close lifecycle (UniTask); `Popup : BaseView` (LitMotion scale); `Screen : BaseView` (LitMotion fade)
     `WinPopupView : Popup` — `[Inject] ResultPopupViewModel`; `ScorePopupView : Popup` — `[Inject] ResultPopupViewModel`; score display via `UniText`
     ⊥ `IUIService` ⊥ `EPopupType`
     `LobbyViewModel` — `ReactiveCommand EnterGame`; ⊥ `ISceneService`; pure C#
     `LobbyScreen : Screen` — single "Enter the Game" btn; placeholder; ∈ Meta/UI/
     `MetaGameSceneReferences : MonoBehaviour, IMetaGameSceneReferences` — ScreensRoot, PopupsRoot, MessagesRoot, Canvas; ∈ Meta/Infrastructure/
- cfg.session: `SessionSettingsConfig.SessionEndTime` (int sec, default 180) + `ResultDelayTime` (float sec, default 2.0)
- cfg.shoot: `ShootingConfig` — MaxReflections, CastOffset, CastDistance, ProjectileLifetimeDuration
- cfg.grid: `GridSettingsConfig` — rows/cols/cell layout

## §V INVARIANTS
V1: shoot ! only when `ShootingPhaseTag.Count > 0` & no flying projectile (`Bubble+Path` entity) in world
V2: projectile land → `ShotLandedEvent` + `RefreshFieldEvent` emitted same frame (`ProjectileReplacementSystem`)
V3: pop group ! only when `indices.Count >= BubblesToPop`
V4: `FieldSettledEvent` emitted ! only when `FieldProcessingPhaseTag` active & `PendingAnimations.Count <= 0`
V5: pipeline order: `FieldSettleCheckSystem` before `GameStateMachineSystem` (same-frame settle detect)
V6: phase tags mutually exclusive — exactly 1 phase tag on state entity at all times
V7: `ShotLandedEvent`, `FieldSettledEvent`, `RefreshFieldEvent` — AutoDel'd (1-frame lifetime)
V8: `EvaluateGameOver` ! check `BoardIsFull` & `BoardIsCleaned` before → `Result` transition
V9: `EndGameResult.Count > 0` → `EvaluateGameOver` returns true → phase transitions to `Result`
V10: `Result` phase entry → wait `ResultDelayTime` seconds → show result overlay Canvas on Core scene
V11: result overlay ! win (`BoardIsCleaned`) → show `WinPopup`; lose (`TimeIsUp`\|`BoardIsFull`) → show `ScorePopup` with `ISessionDataService.Score`
V12: result overlay ! single Submit button → `SceneService` loads Meta scene
V13: pop group of N bubbles → `IScoreService.Add(sum(PopScoreBase + i*PopScoreIncrement, i=0..N-1))`
V14: N dropped bubbles → `IScoreService.Add(N * DropScorePerBubble)`
V16: `ComputeFinal(TimeIsUp, _)` → no time bonus; `ComputeFinal(other, t)` → `Total += floor(A*t² + B*t + C)` where A,B,C = `TimeBonusA/B/C`
V17: `ComputeFinal(BoardIsCleaned, _)` → `Total += BoardClearBonus`
V18: `ResultPhaseSystem` first-frame `ResultPhaseTag` → `IScoreService.ComputeFinal()` + `ISessionDataService.Snapshot()` called synchronously before any async visual delay
V19: `ISessionDataService.Score` & `.IsGameEnded` mutated only inside `Snapshot()`; no external direct writes permitted
V15: `BubblesPoppedEvent{Count}` & `BubblesDroppedEvent{Count}` emitted by `DropAndPopSystem` same frame as pop/drop; AutoDel'd
V20: `IPopupService` has zero knowledge of ViewModel types; ViewModel resolved by VContainer at instantiation — not passed by caller
V21: lifecycle order strict: VContainer injection → `OnOpenStart` → `PlayOpenAnimation` → `OnOpenComplete`; close: `OnCloseStart` → `PlayCloseAnimation` → `OnCloseComplete` → destroy/cache
V22: `IPopupService.Show<TView>()` while TView instance already open → no-op (no double-instantiate, no double-open)
V23: every lifecycle step awaited sequentially (⊥ fire-and-forget); `Show`/`Hide` return `UniTask`
V24: `ScorePopupView` uses `LightSide.UniText` for score display (⊥ TMPro, ⊥ UnityEngine.UI.Text)
V25: double Core-load prevented by Meta scene disabled immediately after load (V28); `LobbyViewModel` ⊥ self-disable
V26: `MetaFlow` owns Core scene load on `EnterGame`; `LobbyViewModel` ⊥ `ISceneService` ⊥ `LifetimeScope`
V27: `IScreenService` ≤1 active Screen at a time; tutorial overlays ∈ separate system (⊥ `IScreenService`)
V28: `MetaFlow` after Core load additive → `SetSceneEnabled(Meta, false)`; subscribes `SceneManager.sceneUnloaded`; on Core unload → `SetSceneEnabled(Meta, true)` + unsubscribes; `IDisposable.Dispose()` removes subscription on MetaScope teardown
V29: `SceneService.LoadSceneAsync` — Empty-buffer (`needLoadEmpty`) ! only when `mode == Single`; Additive loads must never trigger Empty (Single) → would destroy existing scenes + dispose parent LifetimeScope containers
V30: `MoveAlongPathSystem` waypoint advance — detect passage by `dot(nextPoint - position, direction) ≤ 0`; proximity epsilon alone invalid when `speed * dt > sqrt(Epsilon)` → overshoot → direction reversal → collision miss
V31: `GetUnattached` BFS anchor = ∀ occupied cells with `index.x==0` (top row); `GetLineIndices(n)` returns `(n,j)` for all j — a column, not a row; seeding from column y=0 instead of top row x=0 → ceiling-attached groups wrongly classified as unattached → false drop
V32: `GetCollisionPoints` ceiling branch (hit.normal==Vector2.down) must call `FindClosestCell(hit.point, Free)` + assign `index` + append cellPosition to path before break; omitting → index=null → bubble placed at `Vector2Int.zero` (top-left cell)
V33: on `FieldSettledEvent`, if zero Occupied cells remain in grid → `EndGameResult{BoardIsCleaned}` emitted before `EvaluateGameOver()`; missing check → win condition never triggers regardless of board state

## §T TASKS
id|status|task|cites
T1|x|GridSpawnSystem: spawn hex grid cells|-
T2|x|SpawnFieldSystem: fill initial grid random-color bubbles|-
T3|x|CannonModule: aim line + rotation + shoot|I.input,V1
T4|x|MoveAlongPathSystem: move projectile along reflected ray path|V1
T5|x|ProjectileReplacementSystem: land projectile → place bubble → emit events|V2
T6|x|DropAndPopSystem: pop match-group + drop unattached|V3
T7|x|FieldSettleCheckSystem: detect settle → emit FieldSettledEvent|V4,V5
T8|x|GameStateMachineSystem: phase FSM Bootstrap→PreStart→Shooting→FieldProcessing→Result|V5,V6
T9|x|TimerTickSystem: register in CoreFlow pipeline|V8
T10|x|EndGameConditionCheckSystem: register in CoreFlow pipeline & wire SessionEndTime|I.cfg.session,V8
T11|x|EvaluateGameOver: return true when EndGameResult.Count > 0 (TimeIsUp scope only)|V9
T12|x|ResultPhaseSystem: ResultDelayTime delay → show result overlay (win congrats \| lose score)|V10,V11,V12,V18,V19
T13|x|HighlightNeighborCellsSystem: debug-only, not needed|-
T14|x|SessionSettingsConfig: add ResultDelayTime field|I.cfg.session,V10
T15|x|GameplayRulesConfig: add PopScoreBase, PopScoreIncrement, DropScorePerBubble|I.cfg.gameplay
T16|x|DropAndPopSystem: emit BubblesPoppedEvent{Count} + BubblesDroppedEvent{Count}|V15
T17|x|ScoreSystem: accumulate Score world component from pop/drop events|V13,V14,V15
T18|x|CoreFlow: register ScoreSystem in pipeline (after DropAndPopSystem)|I.pipeline
T19|x|Remove `Score` ECS world component; `ScoreSystem` writes to `IScoreService.Add()`|V13,V14,I.score
T20|x|`SessionDataService`: `ReactiveProperty<int> Score`; `Reset()` on session start; registered in DI|I.session
T21|x|`ScoreService`: `Add()` accumulates; `Finalize(reason, timeRemaining)` applies time+clear bonuses → writes `ISessionDataService.Score`|V16,V17,I.score,I.session
T22|x|`GameplayRulesConfig`: add `TimeBonusA/B/C` + `BoardClearBonus`|I.cfg.gameplay,V16,V17
T23|x|Restructure Runtime: Core→Gameplay, sort Services+Configs by layer into Gameplay/ & Shared/, rm Test/|§F
T24|x|`BaseView`: MonoBehaviour open/close lifecycle hooks (UniTask); `Popup : BaseView` LitMotion scale; `Screen : BaseView` LitMotion fade (stub)|V21,V23
T25|x|`IPopupSource` + `PopupSourceConfig : ScriptableObject` — prefab registry, `GetPrefab<TView>()` by type|§I.ui
T26|x|`PopupService : IPopupService` — `IObjectResolver` instantiates TView, caches instance per type, drives lifecycle; `HideAll()` runs close on all active|V20,V21,V22,V23
T27|x|`WinPopupView : Popup`, `ScorePopupView : Popup`; `[Inject] ResultPopupViewModel`; score text via `LightSide.UniText`|V21,V24
T28|x|rm `IUIService` `UIService` `EPopupType`; `ResultPhaseSystem` → `IPopupService.Show<WinPopupView>()` / `Show<ScorePopupView>()`|V11,§I.ui
T29|x|`CoreScope`: register `PopupSourceConfig` as `IPopupSource`, `PopupService` as singleton `IPopupService`; rm old UIService reg|§I.ui
T30|x|`Screen : BaseView` — LitMotion fade open/close; full impl (was stub in T24)|V21,V23
T31|x|`LobbyViewModel` — `ReactiveCommand EnterGame`; disables self after first exec|V25,V26
T32|x|`LobbyScreen : Screen` — "Enter the Game" btn bound to `EnterGame`; placeholder layout|V21,V25
T33|x|`MetaScope` — register `IScreenService` (singleton) + `IViewSourceProvider` (`ViewSourceConfig` w/ LobbyScreen prefab) + `LobbyViewModel`|§I.ui,V27
T34|x|`MetaFlow` — `IScreenService.Show<LobbyScreen>()`; subscribe `LobbyViewModel.EnterGame` → `EnqueueParent(_parent)` + `LoadSceneAsync(Core, Additive)`|V25,V26,V27
T35|x|rename `IPopupSource` → `IViewSourceProvider`, `PopupSourceConfig` → `ViewSourceConfig` across codebase|§I.ui
T36|x|extract `BaseViewService` (abstract); add `IScreenService` + `ScreenService : BaseViewService`; `PopupService` extends `BaseViewService`|§I.ui,V27
T37|x|`MoveAlongPathSystem`: replace proximity epsilon with dot-product waypoint-passage check; fix `return` → `continue` in loop body|V30
T38|x|`GetUnattached`: replace `GetLineIndices(0)` with explicit top-row seed `(0,j)` for j in 0..Columns-1|V31
T39|x|`CannonShootSystem.GetCollisionPoints`: ceiling branch → `FindClosestCell(hit.point, Free)` + set index + append cellPosition to path (mirror occupied-bubble branch)|V32
T40|x|`EndGameConditionCheckSystem`: on `FieldSettledEvent` present, scan grid; if zero Occupied cells → emit `EndGameResult{BoardIsCleaned}`|V33

## §F FOLDER LAYOUT
```
Runtime/
  Bootstrap/           ← app entry; root DI
  Loading/             ← loading scene slice
  Meta/                ← out-of-game scene; UI/ subdir for meta screens
  Gameplay/            ← rename from Core; in-game ECS scene slice
    Infrastructure/    ← CoreFlow CoreScope CoreGameSceneRefs
    Systems/ Components/ Views/ Models/ Modules/ Interfaces/ Constants/ Enums/
    Services/          ← in-game only: Grid Input
    Configs/           ← GameplayRulesConfig ShootingConfig GridSettingsConfig
    UI/                ← result overlay WinPopup ScorePopup
  Shared/              ← cross-scene; used by both Meta & Gameplay
    Services/          ← Scene Loading Session Asset
    Configs/           ← SessionSettingsConfig
    UI/                ← IScreen IPopup base contracts
    Extensions/
    Utilities/
  Generated/
  RuntimeConstants.cs
  ⊥ Test/              ← deleted (prototype residue)
```
layer rule: ECS systems ∈ Gameplay only; services that cross scenes ∈ Shared; popups/screens base ∈ Shared/UI, impls ∈ owning layer

## §Q OPEN QUESTIONS
Q1: RESOLVED — MVVM + R3 for all UI
  - ViewModel: pure C#, zero Unity deps, R3 observable props + commands
  - result overlay: single `ResultPopupViewModel` (Score + Submit) shared by Win+Score views; visual diff (confetti etc.) is View concern only
  - View: MonoBehaviour, binds to ViewModel via R3 subscriptions; swappable via asset bundle (reskin = new prefab, ViewModel untouched)
  - ViewModel gets data from `ISessionDataService`, not from ECS directly
Q2: IPopup.OnClosed event — add now (enables future popup queue) or defer?
Q3: result overlay is IPopup not IScreen (confirmed); IScreen = singleton full-screen, IPopup = stackable on top

## §B BUGS
id|date|cause|fix
B1|2026-05-27|`LoadSceneAsync` applied Empty-buffer for Additive loads → destroyed Meta scene + disposed MetaScope container → second Core load: `EnqueueParent(metaScope)` parent dead → VContainer can't reach BootstrapScope → `ISceneService` not found|V29: gate `needLoadEmpty` on `mode==Single`
B2|2026-05-27|`MoveAlongPathSystem`: sqrMagnitude < 0.01f waypoint check fails when projectile overshoots by > 0.1u → index frozen → direction unchanged → projectile moves away from passed point → reflection skipped → bubble never lands|V30: dot-product passage check; T37
B3|2026-05-27|`ScoreService.ComputeFinal` applies time+clear bonuses silently → tester sees unexpectedly high score (e.g. 3000) with zero pops; no debug breakdown|add `CustomDebugLog` bonus breakdown in `ComputeFinal`
B4|2026-05-27|`GetUnattached` seeds BFS from col y=0 (leftmost) via `GetLineIndices(0)`; correct anchor = top row x=0; bubbles attached only to ceiling (not left wall) wrongly dropped|V31
B5|2026-05-27|`CannonShootSystem.GetCollisionPoints` ceiling branch skips `FindClosestCell` + index assignment → index=null → bubble placed at `Vector2Int.zero` on every ceiling shot|V32
B6|2026-05-27|`EndGameConditionCheckSystem` only emits `TimeIsUp`; no system checks all-Free grid after settle → `EvaluateGameOver()` never true for win → `BoardIsCleaned` never triggered|V33
