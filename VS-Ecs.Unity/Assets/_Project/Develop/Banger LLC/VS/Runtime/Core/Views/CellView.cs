using Unity.Collections;
using UnityEngine;
using VS.Runtime.Core.Enums;

namespace VS.Runtime.Core.Views
{
    public class CellView : View
    {
        [field: SerializeField, ReadOnly] public Vector2Int Coord { get; private set; }
        [field: SerializeField, ReadOnly] public ECellState State { get; private set; }
        [field: SerializeField, ReadOnly] public BoxCollider2D Collider { get; private set; }

        public void SetState(ECellState state) => 
            State = state;

        public void SetCoord(Vector2Int coord) => 
            Coord = coord;
    }
}
