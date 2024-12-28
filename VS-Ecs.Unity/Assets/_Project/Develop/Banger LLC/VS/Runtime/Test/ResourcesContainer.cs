using DCFApixels.DragonECS;
using UnityEngine;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Test
{
    [CreateAssetMenu(fileName = "ResourcesContainer", menuName = "Configs/Data/ResourcesContainer")]
    public class ResourcesContainer : ScriptableObject
    {
        [field: SerializeField] public EcsEntityConnect Bubble { get; private set; }
        [field: SerializeField] public EcsEntityConnect Cannon { get; private set; }
        [field: SerializeField] public EcsEntityConnect AimLine { get; private set; }
    }
}