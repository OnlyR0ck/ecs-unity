using UnityEngine;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Core.Models
{
    public class GridModel
    {
        public GridComponent Grid { get; private set; }

        public void Initialize(ref GridComponent grid)
        {
            Grid = grid;
        }

        public CellView this[Vector2Int index]
        {
            get => Grid.Cells[index.x, index.y];
            set => Grid.Cells[index.x, index.y] = value;
        }
    }
}