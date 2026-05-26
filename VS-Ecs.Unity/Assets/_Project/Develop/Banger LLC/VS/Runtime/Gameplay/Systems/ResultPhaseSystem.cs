using System;
using Cysharp.Threading.Tasks;
using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Components.StateMachine;
using VS.Runtime.Core.UI;
using VS.Runtime.Services.Session;
using VS.Runtime.Shared.UI;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace VS.Runtime.Core.Systems
{
    public class ResultPhaseSystem : IEcsInit, IEcsRun
    {
#if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
        private class ResultAspect : EcsAspect
        {
            public EcsPool<ResultPhaseTag> Tags = Inc;
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
        private class EndGameAspect : EcsAspect
        {
            public EcsPool<EndGameResult> Results = Inc;
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
        private class TimerAspect : EcsAspect
        {
            public EcsPool<Timer> Timers = Inc;
            public EcsPool<EndGameTimerTag> TimerTags = Inc;
        }

        private readonly EcsDefaultWorld _world;
        private readonly SessionSettingsConfig _config;
        private readonly IScoreService _scoreService;
        private readonly ISessionDataService _sessionData;
        private readonly IPopupService _popupService;
        private bool _triggered;

        [Inject]
        public ResultPhaseSystem(
            EcsDefaultWorld world,
            SessionSettingsConfig config,
            IScoreService scoreService,
            ISessionDataService sessionData,
            IPopupService popupService)
        {
            _world = world;
            _config = config;
            _scoreService = scoreService;
            _sessionData = sessionData;
            _popupService = popupService;
        }

        public void Init() { }

        public void Run()
        {
            if (_triggered || _world.Where(out ResultAspect _).Count == 0)
                return;

            _triggered = true;

            var reason = EGameEndReason.TimeIsUp;
            foreach (int e in _world.Where(out EndGameAspect endAspect))
            {
                reason = endAspect.Results.Get(e).GameEndReason;
                break;
            }

            int timeRemaining = 0;
            foreach (int e in _world.Where(out TimerAspect timerAspect))
            {
                timeRemaining = timerAspect.Timers.Get(e).TimeRemaining;
                break;
            }

            // V18: synchronous data commit before visual delay
            _scoreService.ComputeFinal(reason, timeRemaining);
            _sessionData.Snapshot();

            ShowAfterDelayAsync(reason).Forget();
        }

        private async UniTaskVoid ShowAfterDelayAsync(EGameEndReason reason)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.ResultDelayTime));
            if (reason == EGameEndReason.BoardIsCleaned)
                await _popupService.Show<WinPopupView>();
            else
                await _popupService.Show<ScorePopupView>();
        }
    }
}
