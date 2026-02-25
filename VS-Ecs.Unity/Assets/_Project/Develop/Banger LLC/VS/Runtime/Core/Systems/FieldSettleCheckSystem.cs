using DCFApixels.DragonECS;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Components.StateMachine;
using VS.Runtime.Core.Components.OneFrameComponents.Events;

namespace VS.Runtime.Core.Systems
{
    public class FieldSettleCheckSystem : IEcsRun
    {
        private readonly EcsDefaultWorld _world;
        private EcsPool<FieldSettledEvent> _fieldSettledPool;
        private EcsPool<FieldProcessingPhaseTag> _fieldProcessingPool;

        public FieldSettleCheckSystem(EcsDefaultWorld world)
        {
            _world = world;
        }

        public void Run()
        {
            // Only runs in FieldProcessing phase
            if (_world.GetPool<FieldProcessingPhaseTag>().Count == 0)
                return;

            ref var pendingAnims = ref _world.Get<PendingAnimations>();
            bool noAnimations = pendingAnims.Count <= 0;

            if (noAnimations)
            {
                int entity = _world.NewEntity();
                _world.GetPool<FieldSettledEvent>().Add(entity);
                // System will wait for StateMachineSystem to transition phase in the same frame (it should be after this system)
            }
        }
    }
}
