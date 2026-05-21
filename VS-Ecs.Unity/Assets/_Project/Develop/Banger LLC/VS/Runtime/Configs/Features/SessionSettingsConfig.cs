using UnityEngine;

namespace VS.Core.Configs.Features
{
    [CreateAssetMenu(fileName = nameof(SessionSettingsConfig), menuName = "Configs/" + nameof(SessionSettingsConfig))]
    public class SessionSettingsConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int SessionEndTime { get; private set; } = 180;
        [field: SerializeField, Min(0f)] public float ResultDelayTime { get; private set; } = 2f;
    }
}