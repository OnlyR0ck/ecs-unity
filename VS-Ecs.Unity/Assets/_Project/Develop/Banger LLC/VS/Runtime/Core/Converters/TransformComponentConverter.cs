using Arch.Unity.Conversion;
using UnityEngine;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Converters
{
    public class TransformComponentConverter : MonoBehaviour, IComponentConverter
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        
        
        public void Convert(IEntityConverter converter)
        {
            converter.AddComponent
            (
                new TransformComponent(Transform)
            );
        }
    }
}