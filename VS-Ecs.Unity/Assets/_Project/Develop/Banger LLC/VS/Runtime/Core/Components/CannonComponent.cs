using System;
using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct CannonComponent : IEcsComponent
    {
        public Transform AimLineRoot;
        public Transform BulletSpawnRoot;
    }
    
    
    public class CannonTemplate : ComponentTemplate<CannonComponent> { }
}