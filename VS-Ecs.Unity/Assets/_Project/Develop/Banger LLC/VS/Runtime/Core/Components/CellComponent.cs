using DCFApixels.DragonECS;
using UnityEngine;
using VS.Runtime.Core.Enums;

namespace VS.Runtime.Core.Components
{
    public struct CellComponent : IEcsComponent
    {
        public Vector2Int Index;
        public ECellState State;
    }
}