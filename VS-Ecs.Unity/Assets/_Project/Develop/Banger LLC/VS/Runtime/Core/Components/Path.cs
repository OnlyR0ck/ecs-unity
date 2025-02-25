using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    public struct Path : IEcsComponent
    {
        public Vector3[] Points;
        public int CurrentIndex;

        public Path(Vector3[] points)
        {
            Points = points;
            CurrentIndex = 0;
        }
    }

    public class PathComponentTemplate : ComponentTemplate<Path>
    {
        
    }
}