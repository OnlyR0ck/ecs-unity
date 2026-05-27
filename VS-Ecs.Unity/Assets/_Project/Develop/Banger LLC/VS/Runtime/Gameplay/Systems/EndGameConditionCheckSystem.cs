using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Components.OneFrameComponents.Events;
using VS.Runtime.Core.Components.StateMachine;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace VS.Runtime.Core.Systems
{
    public class EndGameConditionCheckSystem : IEcsInit, IEcsRun
    {
#if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
        private class TimerAspect : EcsAspect
        {
            public readonly EcsPool<Timer> Timers = Inc;
            public readonly EcsPool<EndGameTimerTag> Tags = Inc;
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
        private class FieldSettledAspect : EcsAspect
        {
            public readonly EcsPool<FieldSettledEvent> Events = Inc;
        }

        private readonly SessionSettingsConfig _config;
        private readonly EcsDefaultWorld _world;

        [Inject]
        public EndGameConditionCheckSystem(SessionSettingsConfig config, EcsDefaultWorld world)
        {
            _world = world;
            _config = config;
        }

        public void Init()
        {
            entlong entity = _world.NewEntityLong();
            var aspect = _world.GetAspect<TimerAspect>();
            ref var timer = ref aspect.Timers.Add(entity.ID);
            timer.TimeRemaining = _config.SessionEndTime;
            aspect.Tags.Add(entity.ID);
        }

        public void Run()
        {
            if (ShouldSkip())
                return;

            foreach (var entity in _world.Where(out TimerAspect aspect))
            {
                ref var timer = ref aspect.Timers.Get(entity);
                if (timer.TimeRemaining > 0)
                    continue;

                aspect.Timers.Del(entity);
                aspect.Tags.Del(entity);
                var newEntity = _world.NewEntity();
                ref var result = ref _world.GetPool<EndGameResult>().TryAddOrGet(newEntity);
                result.GameEndReason = EGameEndReason.TimeIsUp;
                return;
            }

            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
            {
                var newEntity = _world.NewEntity();
                ref var result = ref _world.GetPool<EndGameResult>().TryAddOrGet(newEntity);
                result.GameEndReason = EGameEndReason.Debug;
            }
            #endif
        }

        // skip end-game check while a FieldSettledEvent is being processed this frame
        private bool ShouldSkip() =>
            _world.Where(out FieldSettledAspect _).Count == 1;
    }
}