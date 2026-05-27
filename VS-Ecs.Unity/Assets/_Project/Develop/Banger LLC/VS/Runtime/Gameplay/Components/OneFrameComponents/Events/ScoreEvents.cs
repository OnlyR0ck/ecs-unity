using System;
using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components.OneFrameComponents.Events
{
    [Serializable]
    public struct BubblesPoppedEvent : IEcsComponent
    {
        public int Count;
    }

    [Serializable]
    public struct BubblesDroppedEvent : IEcsComponent
    {
        public int Count;
    }
}
