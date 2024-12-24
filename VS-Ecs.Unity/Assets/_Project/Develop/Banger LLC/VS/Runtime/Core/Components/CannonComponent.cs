using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    public struct CannonComponent : IEcsComponent
    {
        public Transform AimLineRoot;
        public Transform BulletSpawnRoot;
    }
}