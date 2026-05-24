using VS.Runtime.Core.Components.StateMachine;

namespace VS.Runtime.Services.Session
{
    public interface IScoreService
    {
        void Add(int delta);
        void Finalize(EGameEndReason reason, int timeRemaining);
    }
}