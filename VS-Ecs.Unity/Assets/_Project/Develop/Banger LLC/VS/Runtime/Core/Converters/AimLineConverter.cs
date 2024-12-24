using UnityEngine;

namespace VS.Runtime.Core.Converters
{
    public class AimLineConverter : MonoBehaviour
    {
        [field: SerializeField] public LineRenderer LineRenderer { get; private set; }
    }
}