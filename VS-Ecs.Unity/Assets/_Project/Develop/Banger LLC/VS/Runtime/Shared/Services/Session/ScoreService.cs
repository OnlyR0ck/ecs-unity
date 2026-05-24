using UnityEngine;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components.StateMachine;

namespace VS.Runtime.Services.Session
{
    public class ScoreService : IScoreService
    {
        private readonly GameplayRulesConfig _config;
        private readonly ISessionDataService _sessionData;
        private int _accumulated;

        public ScoreService(GameplayRulesConfig config, ISessionDataService sessionData)
        {
            _config = config;
            _sessionData = sessionData;
        }

        public void Add(int delta) => _accumulated += delta;

        public void Finalize(EGameEndReason reason, int timeRemaining)
        {
            int total = _accumulated;

            if (reason != EGameEndReason.TimeIsUp)
                total += Mathf.FloorToInt(_config.TimeBonusA * timeRemaining * timeRemaining
                                        + _config.TimeBonusB * timeRemaining
                                        + _config.TimeBonusC);

            if (reason == EGameEndReason.BoardIsCleaned)
                total += _config.BoardClearBonus;

            _sessionData.Score.Value = total;
        }
    }
}