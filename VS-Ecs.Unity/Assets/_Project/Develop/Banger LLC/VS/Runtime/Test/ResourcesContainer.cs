using DCFApixels.DragonECS;
using UnityEngine;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Test
{
    [CreateAssetMenu(fileName = "ResourcesContainer", menuName = "Configs/Data/ResourcesContainer")]
    public class ResourcesContainer : ScriptableObject
    {
        [field: SerializeField] public BubbleView Bubble { get; private set; }
        [field: SerializeField] public EcsEntityConnect Cannon { get; private set; }
        [field: SerializeField] public EcsEntityConnect AimLine { get; private set; }
        [field: SerializeField] public BubbleView BubbleCell { get; private set; }
        [field: SerializeField] public GridView GridViewPrefab { get; private set; }
        [field: SerializeField] public EcsEntityConnect CellHighlight { get; private set; }
    }
}