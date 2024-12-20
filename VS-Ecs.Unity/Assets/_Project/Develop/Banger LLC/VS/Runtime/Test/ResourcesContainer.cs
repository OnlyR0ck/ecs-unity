using UnityEngine;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Test
{
    [CreateAssetMenu(fileName = "ResourcesContainer", menuName = "Configs/Data/ResourcesContainer")]
    public class ResourcesContainer : ScriptableObject
    {
        [field: SerializeField] public BubbleView Bubble { get; private set; }
        [field: SerializeField] public GameObject Cube { get; private set; }
        [field: SerializeField] public LineRenderer AimLine { get; private set; }
    }
}