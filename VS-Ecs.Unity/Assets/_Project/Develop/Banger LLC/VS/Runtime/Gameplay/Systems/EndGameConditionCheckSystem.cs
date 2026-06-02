using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Components.OneFrameComponents.Events;
using VS.Runtime.Core.Components.StateMachine;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;

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
        private readonly GridModel _gridModel;

        [Inject]
        public EndGameConditionCheckSystem(SessionSettingsConfig config, EcsDefaultWorld world, GridModel gridModel)
        {
            _world = world;
            _config = config;
            _gridModel = gridModel;
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
            // V33: on FieldSettledEvent, check if board is clear
            if (_world.Where(out FieldSettledAspect _).Count > 0)
            {
                if (IsBoardCleared())
                {
                    var newEntity = _world.NewEntity();
                    ref var result = ref _world.GetPool<EndGameResult>().TryAddOrGet(newEntity);
                    result.GameEndReason = EGameEndReason.BoardIsCleaned;
                }
                return;
            }

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

        //TODO: to zlinq
        private bool IsBoardCleared()
        {
            var cells = _gridModel.Grid.Cells;
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y].State == ECellState.Occupied)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
