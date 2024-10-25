namespace VS.Runtime.Utilities.StateMachine
{
    public abstract class State<T> : IState<T>
    {
        protected ITriggerResponder<T> StateMachine;

        public virtual void OnEnter(ITriggerResponder<T> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void OnExit()
        {
            StateMachine = null;
        }

        public virtual void Dispose()
        {
            StateMachine = null;
        }
    }
}