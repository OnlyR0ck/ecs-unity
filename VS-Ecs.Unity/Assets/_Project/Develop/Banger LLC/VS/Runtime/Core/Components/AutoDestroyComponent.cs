using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components
{
    public struct AutoDestroyComponent : IEcsComponent
    {
        public float TimeToDestroy;

        public AutoDestroyComponent(float timeToDestroy = 5)
        {
            TimeToDestroy = timeToDestroy;
        }
    }
}