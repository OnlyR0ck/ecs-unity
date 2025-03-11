using UnityEngine;

namespace VS.Core.Configs.Features
{
    [CreateAssetMenu(fileName = nameof(GameplayRulesConfig), menuName = "Configs/" + nameof(GameplayRulesConfig))]
    public class GameplayRulesConfig : ScriptableObject
    {
        [field: SerializeField, Min(2)] public int BubblesToPop { get; private set; } = 3;
    }
}