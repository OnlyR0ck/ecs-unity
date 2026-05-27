using System;
using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components.OneFrameComponents.Events
{
    [Serializable]
    public struct ShotLandedEvent : IEcsComponent { }

    [Serializable]
    public struct FieldSettledEvent : IEcsComponent { }
}
