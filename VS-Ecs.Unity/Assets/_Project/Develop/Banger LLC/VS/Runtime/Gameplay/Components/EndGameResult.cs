using System;
using DCFApixels.DragonECS;
using VS.Runtime.Core.Components.StateMachine;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct EndGameResult : IEcsComponent
    {
        public EGameEndReason GameEndReason;
    }
}