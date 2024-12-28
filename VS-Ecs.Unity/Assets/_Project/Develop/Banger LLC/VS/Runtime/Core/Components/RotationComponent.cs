using System;
using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct RotationComponent : IEcsComponent
    {
        public float Speed;
        public Vector2 FromTo;

        public RotationComponent(float speed, Vector2 fromTo)
        {
            Speed = speed;
            FromTo = fromTo;
        }
    }
    
    public class RotationComponentTemplate : ComponentTemplate<RotationComponent> {}
}