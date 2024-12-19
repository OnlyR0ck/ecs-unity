using UnityEngine;

namespace VS.Core.Configs.Features
{
    [CreateAssetMenu(fileName = nameof(ShootingConfig), menuName = "Configs/Features/" + nameof(ShootingConfig))]
    public class ShootingConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxReflections { get; private set; } = 5;
        [field: SerializeField] public float CastOffset { get; private set; } = 0.1f;
        [field: SerializeField] public float CastDistance { get; private set; } = 100;
    }
}