using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Utilities.Debug;
using Vector2 = UnityEngine.Vector2;

namespace VS.Runtime.Extensions
{
    public static class GridExtensions
    {
        private static readonly int[] DxNeighborsEven = { 0, 1, -1, 1, 0, 1 };
        private static readonly int[] DxNeighborsNotEven = { -1, 0, -1, 1, -1, 0 };
        private static readonly int[] DyNeighbors = { 1, 1, 0, 0, -1, -1 };

        public static CellView FindClosestCell(this GridModel model, Vector2 position, ECellState cellState)
        {
            var pos = model.FindClosestCellIndex(position, cellState);
            return model.Grid.Cells[pos.x, pos.y];
        }
        
        public static Vector2Int FindClosestCellIndex(this GridModel model, Vector2 position, ECellState cellState = ECellState.None)
        {
            var grid = model.Grid;
            var closestSqrDistance = ((Vector2)grid.Cells[0, 0].transform.position - position).sqrMagnitude;
            var closestBubble = Vector2Int.zero;
            
            for (var i = 0; i < grid.Cells.GetLength(0); i++)
            {
                for (var j = 0; j < grid.Cells.GetLength(1); j++)
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

                var cell = model.Grid.Cells[i.x, i.y];

                if (cell.State != ECellState.Occupied)
                    continue;

                if (cell.Content is BubbleView bubble && bubble.Color != color)
                    continue;

                GetConnectedSameColoredCells(model, i, ref visited);
            }
        }

        public static IReadOnlyCollection<Vector2Int> GetNeighbors(this GridModel model, Vector2Int index)
        {
            var lengthX = model.Grid.Cells.GetLength(0);
            var lengthY = model.Grid.Cells.GetLength(1);
            var neighbors = new List<Vector2Int>();

            for (var i = 0; i < DxNeighborsEven.Length; i++)
            {
                var xOffset = model.Grid.StartIsEven == (index.x % 2 == 0) ? DxNeighborsEven[i] : DxNeighborsNotEven[i]; 
                var newX = index.x + DyNeighbors[i];
                var newY = index.y + xOffset;

                if (newX >= 0 && newX < lengthX && newY >= 0 && newY < lengthY)
                {
                    neighbors.Add(new Vector2Int(newX, newY));
                }
            }

            return neighbors;
        }

        public static IReadOnlyCollection<Vector2Int> GetLineIndices(this GridModel model, int lineIndex)
        {
            if (lineIndex > model.Grid.Cells.GetLength(1))
                return null;
            
            var result = new List<Vector2Int>();
            for (var i = 0; i < model.Grid.Cells.GetLength(0); i++)
            {
                result.Add(new Vector2Int(i, lineIndex));
            }

            return result;
        }
        
        public static IReadOnlyCollection<Vector2Int> GetUnattached(this GridModel model)
        {
            // Get the first row's occupied indices
            var firstLineIndices = model.GetLineIndices(0).ToList();
            model.RemoveCellWithState(ref firstLineIndices, ECellState.Free);

            // HashSet to store attached bubbles
            var attached = new HashSet<Vector2Int>(firstLineIndices);

            // Find all attached bubbles using an iterative BFS approach
            model.FindAttachedBubbles(ref attached);

            // Collect all occupied bubbles that are NOT in the attached set
            var unattached = new List<Vector2Int>();
            var width = model.Grid.Cells.GetLength(0);
            var height = model.Grid.Cells.GetLength(1);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var index = new Vector2Int(x, y);
                    if (model.Grid.Cells[x, y].State == ECellState.Occupied && !attached.Contains(index))
                    {
                        unattached.Add(index);
                    }
                }
            }

            return unattached;
        }

// Iterative BFS to prevent recursion overflow
        public static void FindAttachedBubbles(this GridModel model, ref HashSet<Vector2Int> attached)
        {
            var queue = new Queue<Vector2Int>(attached);

            while (queue.Count > 0)
            {
                var index = queue.Dequeue();
                var neighbors = model.GetNeighbors(index).ToList();

                model.RemoveCellWithState(ref neighbors, ECellState.Free);

                foreach (var neighbor in neighbors)
                {
                    if (attached.Add(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

// Optimized version of RemoveCellWithState
        public static void RemoveCellWithState(this GridModel model, ref List<Vector2Int> indices, ECellState state)
        {
            indices.RemoveAll(index => model[index].State == state);
        }
    }
}

