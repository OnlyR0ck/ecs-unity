using UnityEngine;

namespace VS.Runtime.Core.Converters
{
    public class TransformComponentConverter : MonoBehaviour
    {
        [field: SerializeField] public Transform Transform { get; private set; }
    }
}