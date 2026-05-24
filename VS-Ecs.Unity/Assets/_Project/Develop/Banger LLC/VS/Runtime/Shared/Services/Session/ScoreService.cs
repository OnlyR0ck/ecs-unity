using UnityEngine;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components.StateMachine;

namespace VS.Runtime.Services.Session
{
    public class ScoreService : IScoreService
    {
        private readonly GameplayRulesConfig _config;
        private int _accumulated;

        public int Total { get; private set; }

        public ScoreService(GameplayRulesConfig config)
        {
            _config = config;
        }

        public void Add(int delta) => _accumulated += delta;

        public void ComputeFinal(EGameEndReason reason, int timeRemaining)
        {
            int total = _accumulated;

            if (reason != EGameEndReason.TimeIsUp)
                total += Mathf.FloorToInt(_config.TimeBonusA * timeRemaining * timeRemaining
                                        + _config.TimeBonusB * timeRemaining
                                        + _config.TimeBonusC);

            if (reason == EGameEndReason.BoardIsCleaned)
                total += _config.BoardClearBonus;

            Total = total;
        }

        public void Reset()
        {
            _accumulated = 0;
            Total = 0;
        }
    }
}