using System;
using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct Rotation : IEcsComponent
    {
        public float Speed;
        public Vector2 FromTo;

        public Rotation(float speed, Vector2 fromTo)
        {
            Speed = speed;
            FromTo = fromTo;
        }
    }
    
    public class RotationComponentTemplate : ComponentTemplate<Rotation> {}
}