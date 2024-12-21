using UnityEngine;

namespace VS.Runtime.Core.Components
{
    public struct PathComponent
    {
        public readonly Vector3[] Points;
        public int CurrentIndex;

        public PathComponent(Vector3[] points)
        {
            Points = points;
            CurrentIndex = 0;
        }
    }
}