using System;
using DCFApixels.DragonECS;
using UnityEngine;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct ProjectileToCell : IEcsComponent
    {
        public EBubbleColor Color;
        public Vector2Int Index;
    }
}