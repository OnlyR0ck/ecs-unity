using UnityEngine;

namespace VS.Core.Configs.Features
{
    [CreateAssetMenu(fileName = nameof(SessionSettingsConfig), menuName = "Configs/" + nameof(SessionSettingsConfig))]
    public class SessionSettingsConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int SessionEndTime { get; private set; } = 180;
    }
}