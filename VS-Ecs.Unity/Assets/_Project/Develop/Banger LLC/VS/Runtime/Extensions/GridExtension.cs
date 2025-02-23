using UnityEngine;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Extensions
{
    public static class GridExtensions
    {
        public static bool IsBubbleHit(this GridModel model, Vector2 rayOrigin, Vector2 rayDirection, out Vector2[] points, Vector2 cellSize)
        {
            points = new Vector2[2];
            float closestDistance = float.MaxValue;
            bool hitBubble = false;
            var grid = model.Grid;

            float bubbleRadius = cellSize.x / 2.0f;

            for (int i = 0; i < grid.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Cells.GetLength(1); j++)
                {
                    Vector2 bubble = grid.Cells[i, j].transform.position;
                    Vector2 vectorToBubble = bubble - rayOrigin;
                    float projectionLength = Vector2.Dot(vectorToBubble, rayDirection.normalized);

                    // Skip if bubble is behind ray origin
                    if (projectionLength < 0)
                        continue;

                    // Find closest point on ray to this bubble
                    Vector2 closestPointOnRay = rayOrigin + rayDirection.normalized * projectionLength;
                    float distanceToBubble = Vector2.Distance(closestPointOnRay, bubble);

                    // Check if ray hits this bubble
                    if (distanceToBubble <= bubbleRadius && grid.Cells[i, j].State == ECellState.Occupied)
                    {
                        Debug.Log($"Suspect: ({i}, {j})");
                        /*if (!RayCircleIntersection.FindIntersection(rayOrigin, rayDirection.normalized, bubble, bubbleRadius,
                                out float x, out float y))*/
                        if (!RayCircleIntersection.FindIntersection(rayOrigin, rayDirection.normalized, bubble, bubbleRadius,
                                out Vector2 ip))
                        {
                            return false;
                        };

                        Vector2 collisionPoint = ip;
                        Vector2Int closestIndex = Vector2Int.zero;
                        closestDistance = ((Vector2)grid.Cells[closestIndex.x, closestIndex.y].transform.position 
                                                 - collisionPoint).sqrMagnitude;

                        for (int k = 0; k < grid.Cells.GetLength(0); k++)
                        {
                            for (int l = 0; l < grid.Cells.GetLength(1); l++)
                            {
                                if (grid.Cells[k, l].State != ECellState.Free)
                                    continue;
                                
                                var position = (Vector2)grid.Cells[k, l].transform.position;
                                var distance = (position - collisionPoint).sqrMagnitude;
                                if (distance > closestDistance)
                                    continue;

                                closestDistance = distance;
                                closestIndex = new Vector2Int(k, l);
                            }
                        }

                        points[0] = collisionPoint;
                        points[1] = grid.Cells[closestIndex.x, closestIndex.y].transform.position;
                        return true;
                        // Only consider empty cells and closer hits
                        /*if (grid.Cells[i, j].State == ECellState.Free && projectionLength < closestDistance)
                        {
                            closestDistance = projectionLength;
                            closestEmptyBubble = closestPointOnRay;
                        }*/
                    }
                }
            }

            return false;
        }


        public static CellView FindClosestCell(this GridModel model, Vector2 position, ECellState cellState)
        {
            Vector2Int i = model.FindClosestCellIndex(position, cellState);
            return model.Grid.Cells[i.x, i.y];
        }
        
        public static Vector2Int FindClosestCellIndex(this GridModel model, Vector2 position, ECellState cellState)
        {
            var grid = model.Grid;
            float closestSqrDistance = ((Vector2)grid.Cells[0, 0].transform.position - position).sqrMagnitude;
            Vector2Int closestBubble = Vector2Int.zero;
            
            for (int i = 0; i < grid.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Cells.GetLength(1); j++)
                {
                    if (grid.Cells[i, j].State != cellState)
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
    }
}

