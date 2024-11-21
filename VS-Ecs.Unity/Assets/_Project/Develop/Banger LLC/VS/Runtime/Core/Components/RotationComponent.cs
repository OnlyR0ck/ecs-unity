using UnityEngine;

namespace VS.Runtime.Core.Components
{
    public struct RotationComponent
    {
        public readonly float Speed;
        public readonly Vector2 FromTo;

        public RotationComponent(float speed, Vector2 fromTo)
        {
            Speed = speed;
            FromTo = fromTo;
        }
    }
}