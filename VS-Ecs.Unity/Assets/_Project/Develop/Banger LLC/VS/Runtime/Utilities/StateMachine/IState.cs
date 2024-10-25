using System;

namespace VS.Runtime.Utilities.StateMachine
{
    public interface IState<T> : IDisposable
    {
        public void OnEnter(ITriggerResponder<T> stateMachine);

        public void OnExit();
    }
}