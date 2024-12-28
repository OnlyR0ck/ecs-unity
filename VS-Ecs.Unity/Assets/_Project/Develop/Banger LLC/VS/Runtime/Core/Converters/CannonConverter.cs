using UnityEngine;

namespace VS.Runtime.Core.Converters
{
    public class CannonConverter : MonoBehaviour
    {
        [field: SerializeField] public Transform BulletSpawnTransform { get; private set; }
        [field: SerializeField] public Transform AimLineRoot { get; private set; }
    }
}