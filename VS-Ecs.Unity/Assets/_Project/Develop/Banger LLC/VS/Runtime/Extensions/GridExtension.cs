using System.Collections.Generic;
using UnityEngine;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Utilities.Debug;

namespace VS.Runtime.Extensions
{
    public static class GridExtensions
    {
        private static readonly int[] DxNeighborsEven = { 0, 1, -1, 1, 0, 1 };
        private static readonly int[] DxNeighborsNotEven = { -1, 0, -1, 1, -1, 0 };
        
        private static readonly int[] DyNeighbors = { 1, 1, 0, 0, -1, -1 };

        public static CellView FindClosestCell(this GridModel model, Vector2 position, ECellState cellState)
        {
            Vector2Int i = model.FindClosestCellIndex(position, cellState);
            return model.Grid.Cells[i.x, i.y];
        }
        
        public static Vector2Int FindClosestCellIndex(this GridModel model, Vector2 position, ECellState cellState = ECellState.None)
        {
            var grid = model.Grid;
            float closestSqrDistance = ((Vector2)grid.Cells[0, 0].transform.position - position).sqrMagnitude;
            Vector2Int closestBubble = Vector2Int.zero;
            
            for (int i = 0; i < grid.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Cells.GetLength(1); j++)
                {
                    if (grid.Cells[i, j].State != cellState && cellState != ECellState.None)
                        continue;
                    
                    var currentSqrDistance = ((Vector2)grid.Cells[i, j].transform.position - position).sqrMagnitude;
                    if (currentSqrDistance <= closestSqrDistance)
                    {
                        closestBubble = new Vector2Int(i, j);
                        closestSqrDistance = currentSqrDistance;
                    }
                }
            }

            return closestBubble;
        }

        public static void GetConnectedSameColoredCells(this GridModel model, Vector2Int index, ref HashSet<Vector2Int> visited)
        {
            if (visited.Contains(index))
                return;

            if (model.Grid.Cells[index.x, index.y].Content is not BubbleView view)
            {
                return;
            }

            var color = view.Color;
            visited.Add(index);

            foreach (var i in model.GetNeighbors(index))
            {
                if (visited.Contains(i))
                    continue;

                CellView cell = model.Grid.Cells[i.x, i.y];

                if (cell.State != ECellState.Occupied)
                    continue;

                if (cell.Content is BubbleView bubble && bubble.Color != color)
                    continue;

                GetConnectedSameColoredCells(model, i, ref visited);
            }
        }

        public static IReadOnlyCollection<Vector2Int> GetNeighbors(this GridModel model, Vector2Int index)
        {
            int lengthX = model.Grid.Cells.GetLength(0);
            int lengthY = model.Grid.Cells.GetLength(1);
            var neighbors = new List<Vector2Int>();

            for (int i = 0; i < DxNeighborsEven.Length; i++)
            {
                int xOffset = model.Grid.StartIsEven == (index.x % 2 == 0) ? DxNeighborsEven[i] : DxNeighborsNotEven[i]; 
                int newX = index.x + DyNeighbors[i];
                int newY = index.y + xOffset;

                if (newX >= 0 && newX < lengthX && newY >= 0 && newY < lengthY)
                {
                    neighbors.Add(new Vector2Int(newX, newY));
                }
            }

            return neighbors;
        }
    }
}

