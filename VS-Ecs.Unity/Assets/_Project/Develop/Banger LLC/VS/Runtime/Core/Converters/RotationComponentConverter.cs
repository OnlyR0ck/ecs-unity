using Arch.Unity.Conversion;
using UnityEngine;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Converters
{
    public class RotationComponentConverter : MonoBehaviour, IComponentConverter
    {
        [field: SerializeField, Min(0.1f)] public float Speed { get; private set; }
        [field: SerializeField] public Vector2 FromTo { get; private set; }
        
        public void Convert(IEntityConverter converter)
        {
            converter.AddComponent
            (
                new RotationComponent(Speed, FromTo)
            );
        }
    }
}