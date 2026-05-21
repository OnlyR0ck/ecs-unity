# SPEC

## §G GOAL
Bubble-shooter (VS): player fires colored bubbles at hex grid; match ≥N same-color → pop group + drop unattached. Session ends: timer expires | board fills | board clears.

## §C CONSTRAINTS
- Unity 2D, orthographic camera
- DragonECS (DCFApixels) — ∀ simulation logic in ECS systems
- VContainer — DI; systems resolved Transient
- UniTask — async animations
- LitMotion — tweens (⊥ DOTween)
- `#if ENABLE_IL2CPP` guards on `EcsAspect` inner classes
- `COMPANYNAME_PROD` define → strips debug logs from build
- ⊥ automated tests

## §I INTERFACES
- input: `IInputService.OnEndDrag(Vector2)` → shot trigger
- grid: `IGridParamsService.Params` → `GridParams` (CellSize, Rows, Columns, StartOffset, StartIsEven, VisibleRows)
- level: `ILevel.GridSpawnRoot` — spawn parent transform
- scene: `ICoreGameSceneRefs.GameCamera`
- pipeline: `CoreFlow` registers ∀ ECS systems; drives `EcsPipeline.Run()` each `Tick()`
- cfg.gameplay: `GameplayRulesConfig.BubblesToPop` (int ≥2, default 3) + `PopScoreBase` (int) + `PopScoreIncrement` (int) + `DropScorePerBubble` (int)
- score: `Score` world component `{ int Total }` — accumulated by `ScoreSystem`, read by result UI
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
V11: result overlay ! win (`BoardIsCleaned`) → "Congrats" copy; lose (`TimeIsUp`\|`BoardIsFull`) → neutral `Score.Total` display
V12: result overlay ! single Submit button → `SceneService` loads Meta scene
V13: pop group of N bubbles → `Score.Total += sum(PopScoreBase + i*PopScoreIncrement, i=0..N-1)`
V14: N dropped bubbles → `Score.Total += N * DropScorePerBubble`
V15: `BubblesPoppedEvent{Count}` & `BubblesDroppedEvent{Count}` emitted by `DropAndPopSystem` same frame as pop/drop; AutoDel'd

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
T11|.|EvaluateGameOver: return true when EndGameResult.Count > 0 (TimeIsUp scope only)|V9
T12|.|ResultPhaseSystem: ResultDelayTime delay → show result overlay (win congrats \| lose score)|V10,V11,V12
T13|.|HighlightNeighborCellsSystem: register in CoreFlow pipeline|-
T14|.|SessionSettingsConfig: add ResultDelayTime field|I.cfg.session,V10
T15|x|GameplayRulesConfig: add PopScoreBase, PopScoreIncrement, DropScorePerBubble|I.cfg.gameplay
T16|x|DropAndPopSystem: emit BubblesPoppedEvent{Count} + BubblesDroppedEvent{Count}|V15
T17|x|ScoreSystem: accumulate Score world component from pop/drop events|V13,V14,V15
T18|x|CoreFlow: register ScoreSystem in pipeline (after DropAndPopSystem)|I.pipeline

## §B BUGS
id|date|cause|fix
