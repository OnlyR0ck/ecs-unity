using DCFApixels.DragonECS;
using VS.Runtime.Core.Components.StateMachine;
using VS.Runtime.Core.Components.OneFrameComponents.Events;
using VS.Runtime.Utilities.Debug;

namespace VS.Runtime.Core.Systems
{
    public class GameStateMachineSystem : IEcsInit, IEcsRun
    {
        private readonly EcsDefaultWorld _world;
        private EcsPool<GameStateMachine> _stateMachinePool;
        private EcsPool<BootstrapPhaseTag> _bootstrapPool;
        private EcsPool<PreStartPhaseTag> _preStartPool;
        private EcsPool<ShootingPhaseTag> _shootingPool;
        private EcsPool<FieldProcessingPhaseTag> _fieldProcessingPool;
        private EcsPool<ResultPhaseTag> _resultPool;

        private EcsPool<ShotLandedEvent> _shotLandedPool;
        private EcsPool<FieldSettledEvent> _fieldSettledPool;

        private class StateMachineAspect : EcsAspect
        {
            public EcsPool<GameStateMachine> StateMachines = Inc;
        }

        public GameStateMachineSystem(EcsDefaultWorld world)
        {
            _world = world;
        }

        public void Init()
        {
            _stateMachinePool = _world.GetPool<GameStateMachine>();
            _bootstrapPool = _world.GetPool<BootstrapPhaseTag>();
            _preStartPool = _world.GetPool<PreStartPhaseTag>();
            _shootingPool = _world.GetPool<ShootingPhaseTag>();
            _fieldProcessingPool = _world.GetPool<FieldProcessingPhaseTag>();
            _resultPool = _world.GetPool<ResultPhaseTag>();

            _shotLandedPool = _world.GetPool<ShotLandedEvent>();
            _fieldSettledPool = _world.GetPool<FieldSettledEvent>();

            // Create the state entity
            int entity = _world.NewEntity();
            _stateMachinePool.Add(entity).Current = EGamePhase.Bootstrap;
            _bootstrapPool.Add(entity);
            
            CustomDebugLog.Log("GameStateMachineSystem initialized. Current phase: Bootstrap");
        }

        public void Run()
        {
            foreach (int entity in _world.Where(out StateMachineAspect aspect))
            {
                ref var sm = ref aspect.StateMachines.Get(entity);

                switch (sm.Current)
                {
                    case EGamePhase.Bootstrap:
                        TransitionTo(entity, EGamePhase.PreStart);
                        break;

                    case EGamePhase.PreStart:
                        TransitionTo(entity, EGamePhase.Shooting);
                        break;

                    case EGamePhase.Shooting:
                        if (_shotLandedPool.Count > 0)
                        {
                            TransitionTo(entity, EGamePhase.FieldProcessing);
                        }
                        break;

                    case EGamePhase.FieldProcessing:
                        if (_fieldSettledPool.Count > 0)
                        {
                            // Stub for win/lose logic. For now, transition back to Shooting.
                            bool isGameOver = EvaluateGameOver();
                            TransitionTo(entity, isGameOver ? EGamePhase.Result : EGamePhase.Shooting);
                        }
                        break;

                    case EGamePhase.Result:
                        // Result UI handled elsewhere (based on ResultPhaseTag)
                        break;
                }
            }
        }

        private void TransitionTo(int entity, EGamePhase nextPhase)
        {
            ref var sm = ref _stateMachinePool.Get(entity);
            EGamePhase prevPhase = sm.Current;
            
            RemovePhaseTag(entity, prevPhase);
            sm.Current = nextPhase;
            AddPhaseTag(entity, nextPhase);

            CustomDebugLog.Log($"Phase Transition: {prevPhase} -> {nextPhase}");
        }

        private void AddPhaseTag(int entity, EGamePhase phase)
        {
            switch (phase)
            {
                case EGamePhase.Bootstrap: _bootstrapPool.TryAddOrGet(entity); break;
                case EGamePhase.PreStart: _preStartPool.TryAddOrGet(entity); break;
                case EGamePhase.Shooting: _shootingPool.TryAddOrGet(entity); break;
                case EGamePhase.FieldProcessing: _fieldProcessingPool.TryAddOrGet(entity); break;
                case EGamePhase.Result: _resultPool.TryAddOrGet(entity); break;
            }
        }

        private void RemovePhaseTag(int entity, EGamePhase phase)
        {
            switch (phase)
            {
                case EGamePhase.Bootstrap: _bootstrapPool.TryDel(entity); break;
                case EGamePhase.PreStart: _preStartPool.TryDel(entity); break;
                case EGamePhase.Shooting: _shootingPool.TryDel(entity); break;
                case EGamePhase.FieldProcessing: _fieldProcessingPool.TryDel(entity); break;
                case EGamePhase.Result: _resultPool.TryDel(entity); break;
            }
        }

        private bool EvaluateGameOver()
        {
            // Future implementation: check for win/lose conditions.
            return false;
        }
    }
}
