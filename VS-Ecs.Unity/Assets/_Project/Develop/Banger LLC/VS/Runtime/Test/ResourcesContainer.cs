using UnityEngine;

namespace VS.Runtime.Test
{
    [CreateAssetMenu(fileName = "ResourcesContainer", menuName = "Configs/Data/ResourcesContainer")]
    public class ResourcesContainer : ScriptableObject
    {
        [field: SerializeField] public GameObject Cube { get; private set; }
    }
}