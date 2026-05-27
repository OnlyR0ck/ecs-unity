using System;
using UnityEngine;

namespace VS.Runtime.Core.Components.OneFrameComponents.Events
{
    using DCFApixels.DragonECS;
    #if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    #endif

    [Serializable]
    public struct RefreshFieldEvent : IEcsComponent
    {
        public Vector2Int Index;
    }
}