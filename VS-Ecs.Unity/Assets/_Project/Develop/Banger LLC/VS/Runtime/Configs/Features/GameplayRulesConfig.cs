using UnityEngine;

namespace VS.Core.Configs.Features
{
    [CreateAssetMenu(fileName = nameof(GameplayRulesConfig), menuName = "Configs/" + nameof(GameplayRulesConfig))]
    public class GameplayRulesConfig : ScriptableObject
    {
        [field: SerializeField, Min(2)] public int BubblesToPop { get; private set; } = 3;
        [field: SerializeField, Min(0)] public int PopScoreBase { get; private set; } = 10;
        [field: SerializeField, Min(0)] public int PopScoreIncrement { get; private set; } = 2;
        [field: SerializeField, Min(0)] public int DropScorePerBubble { get; private set; } = 5;
        [field: SerializeField] public float TimeBonusA { get; private set; } = 0.1f;
        [field: SerializeField] public float TimeBonusB { get; private set; } = 0f;
        [field: SerializeField] public float TimeBonusC { get; private set; } = 0f;
        [field: SerializeField, Min(0)] public int BoardClearBonus { get; private set; } = 500;
    }
}