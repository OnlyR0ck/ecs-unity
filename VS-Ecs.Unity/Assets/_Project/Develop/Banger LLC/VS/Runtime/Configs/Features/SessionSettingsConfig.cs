using UnityEngine;

namespace VS.Core.Configs.Features
{
    [CreateAssetMenu(fileName = nameof(GameplayRulesConfig), menuName = "Configs/" + nameof(GameplayRulesConfig))]
    public class SessionSettingsConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int SessionEndTime { get; private set; } = 180;
    }
}