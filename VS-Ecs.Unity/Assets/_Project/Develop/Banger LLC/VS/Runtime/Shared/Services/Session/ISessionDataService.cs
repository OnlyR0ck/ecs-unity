using R3;

namespace VS.Runtime.Services.Session
{
    public interface ISessionDataService
    {
        ReactiveProperty<int> Score { get; }
        void Reset();
    }
}