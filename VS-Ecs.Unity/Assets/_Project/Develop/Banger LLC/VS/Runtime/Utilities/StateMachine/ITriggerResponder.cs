namespace VS.Runtime.Utilities.StateMachine
{
    public interface ITriggerResponder<TTrigger>
    {
        public void FireTrigger(TTrigger trigger);
    }
}