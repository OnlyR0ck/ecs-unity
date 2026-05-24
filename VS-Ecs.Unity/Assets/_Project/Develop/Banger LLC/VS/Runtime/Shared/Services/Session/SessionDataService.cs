using R3;

namespace VS.Runtime.Services.Session
{
    public class SessionDataService : ISessionDataService
    {
        private readonly IScoreService _scoreService;
        private readonly ReactiveProperty<int> _score = new(0);

        public ReadOnlyReactiveProperty<int> Score => _score;
        public bool IsGameEnded { get; private set; }

        public SessionDataService(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        public void Snapshot()
        {
            _score.Value = _scoreService.Total;
            IsGameEnded = true;
        }

        public void Reset()
        {
            _score.Value = 0;
            IsGameEnded = false;
            _scoreService.Reset();
        }
    }
}