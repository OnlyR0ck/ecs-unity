using Arch.Unity.Conversion;
using UnityEngine;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Converters
{
    public class CannonConverter : MonoBehaviour, IComponentConverter
    {
        [field: SerializeField] public Transform BulletSpawnTransform { get; private set; }
        [field: SerializeField] public Transform AimLineRoot { get; private set; }
        
        public void Convert(IEntityConverter converter)
        {
            converter.AddComponent(new Cannon
            {
                BulletSpawnRoot = BulletSpawnTransform,
                AimLineRoot = AimLineRoot
            });
        }
    }
}