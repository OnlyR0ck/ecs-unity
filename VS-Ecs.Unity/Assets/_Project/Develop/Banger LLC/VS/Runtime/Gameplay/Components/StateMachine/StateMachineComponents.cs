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

    public enum EGameEndReason
    {
        TimeIsUp = 0,
        BoardIsFull = 1,
        BoardIsCleaned = 2,
        Debug = 3
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
}
