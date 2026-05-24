using UnityEngine;
using VS.Runtime.Core.Views;

namespace VS.Core.Configs.Features
{
    [CreateAssetMenu(fileName = nameof(GridSettingsConfig), menuName = "Configs/Features/" + nameof(GridSettingsConfig))]
    public class GridSettingsConfig : ScriptableObject
    {
        [field: SerializeField] public int Rows { get; private set; } = 5;
        [field: SerializeField] public int VisibleRows { get; private set; } = 5;
        [field: SerializeField] public int Columns { get; private set; } = 8;
        [field: SerializeField] public int OffsetColumns { get; private set; } = 1;
        [field: SerializeField] public bool StartIsEven { get; private set; } = true;
        [field: SerializeField] public CellView CellViewPrefab { get; private set; }

        [Tooltip("In cell size percents")]
        [field: SerializeField, Range(0, 1)] public float StartOffset { get; private set; }
    }
}