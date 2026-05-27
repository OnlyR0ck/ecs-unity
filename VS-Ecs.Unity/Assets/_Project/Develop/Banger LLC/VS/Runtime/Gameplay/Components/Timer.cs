using System;
using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct Timer : IEcsComponent
    {
        /// <summary>
        /// Time remaining in seconds
        /// </summary>
        public int TimeRemaining;
    }
}