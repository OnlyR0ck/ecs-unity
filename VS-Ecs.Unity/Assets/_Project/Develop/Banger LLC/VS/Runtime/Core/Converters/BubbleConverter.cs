using UnityEngine;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Core.Converters
{
    public class BubbleConverter : MonoBehaviour
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public BubbleView View { get; private set; }
    }
}
