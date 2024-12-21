namespace VS.Runtime.Core.Components
{
    public struct AutoDestroyComponent
    {
        public float TimeToDestroy;

        public AutoDestroyComponent(float timeToDestroy = 5)
        {
            TimeToDestroy = timeToDestroy;
        }
    }
}