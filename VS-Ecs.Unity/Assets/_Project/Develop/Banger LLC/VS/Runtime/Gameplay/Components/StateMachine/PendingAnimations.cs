using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components.StateMachine
{
    public struct PendingAnimations : IEcsWorldComponent<PendingAnimations>
    {
        public int Count;
        public void Init(ref PendingAnimations component, EcsWorld world) { component.Count = 0; }
        public void OnDestroy(ref PendingAnimations component, EcsWorld world) { }
    }
}