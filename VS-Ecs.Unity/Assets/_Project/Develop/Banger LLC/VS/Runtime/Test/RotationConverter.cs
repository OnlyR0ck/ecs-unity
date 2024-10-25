using Arch.Unity.Conversion;
using UnityEngine;

namespace VS.Runtime.Test
{
    public class RotationConverter : MonoBehaviour, IComponentConverter
    {
        [SerializeField] private float _speed = 1.0f;

        public void Convert(IEntityConverter converter)
        {
            converter.AddComponent(new RotationComponent
            {
                Rotation = transform.rotation,
                Speed = _speed
            });
        }
    }
}