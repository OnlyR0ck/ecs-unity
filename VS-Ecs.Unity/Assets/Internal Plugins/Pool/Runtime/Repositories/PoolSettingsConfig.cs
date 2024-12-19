using UnityEngine;

namespace VS.Pool.Repositories
{
    [CreateAssetMenu(fileName = nameof(PoolSettingsConfig), menuName = "VS/Configs/Pool/" + nameof(PoolSettingsConfig))]
    public class PoolSettingsConfig : ScriptableObject
    {
        [field: SerializeField] public string RepositoryPath { get; private set; } = "Assets/Configs/PoolRepositories";
        [field: SerializeField] public string GeneratedScriptPath { get; private set; } = "Assets/Scripts/Generated";
    }
}