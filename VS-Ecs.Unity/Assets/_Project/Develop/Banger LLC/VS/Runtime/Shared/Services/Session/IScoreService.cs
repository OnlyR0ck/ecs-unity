using VS.Runtime.Core.Components.StateMachine;

namespace VS.Runtime.Services.Session
{
    public interface IScoreService
    {
        void Add(int delta);
        void ComputeFinal(EGameEndReason reason, int timeRemaining);
        int Total { get; }
        void Reset();
    }
}