using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    public struct PathComponent : IEcsComponent
    {
        public Vector3[] Points;
        public int CurrentIndex;
    }

    public class PathComponentTemplate : ComponentTemplate<PathComponent>
    {
        
    }
}