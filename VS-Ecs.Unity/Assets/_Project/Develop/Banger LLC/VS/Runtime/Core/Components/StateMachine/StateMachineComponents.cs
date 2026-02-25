using System;
using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components.StateMachine
{
    public enum EGamePhase
    {
        None = 0,
        Bootstrap = 1,
        PreStart = 2,
        Shooting = 3,
        FieldProcessing = 4,
        Result = 5
    }

    [Serializable]
    public struct GameStateMachine : IEcsComponent
    {
        public EGamePhase Current;
    }

    public struct BootstrapPhaseTag : IEcsComponent { }
    public struct PreStartPhaseTag : IEcsComponent { }
    public struct ShootingPhaseTag : IEcsComponent { }
    public struct FieldProcessingPhaseTag : IEcsComponent { }
    public struct ResultPhaseTag : IEcsComponent { }

    public struct PendingAnimations : IEcsWorldComponent<PendingAnimations>
    {
        public int Count;
        public void Init(ref PendingAnimations component, EcsWorld world) { component.Count = 0; }
        public void OnDestroy(ref PendingAnimations component, EcsWorld world) { }
    }
}
