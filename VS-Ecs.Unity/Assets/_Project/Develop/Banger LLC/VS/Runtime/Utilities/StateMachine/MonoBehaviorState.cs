using UnityEngine;

namespace VS.Runtime.Utilities.StateMachine
{
    public class MonoBehaviorState<T> : MonoBehaviour, IState<T>
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