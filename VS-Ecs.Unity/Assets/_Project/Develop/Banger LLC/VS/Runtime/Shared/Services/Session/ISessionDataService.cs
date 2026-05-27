using R3;

namespace VS.Runtime.Services.Session
{
    public interface ISessionDataService
    {
        ReadOnlyReactiveProperty<int> Score { get; }
        bool IsGameEnded { get; }
        void Snapshot();
        void Reset();
    }
}