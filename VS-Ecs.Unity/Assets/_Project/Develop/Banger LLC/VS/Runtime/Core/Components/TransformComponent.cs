using UnityEngine;

namespace VS.Runtime.Core.Components
{
    public struct TransformComponent
    {
        public readonly Transform Transform;

        public TransformComponent(Transform transform)
        {
            Transform = transform;
        }
    }
}