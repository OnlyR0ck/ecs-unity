using R3;

namespace VS.Runtime.Services.Session
{
    public class SessionDataService : ISessionDataService
    {
        public ReactiveProperty<int> Score { get; } = new(0);

        public void Reset() => Score.Value = 0;
    }
}