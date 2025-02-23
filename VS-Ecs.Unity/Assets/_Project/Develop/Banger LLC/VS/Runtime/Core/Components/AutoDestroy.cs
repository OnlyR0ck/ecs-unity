using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components
{
    public struct AutoDestroy : IEcsComponent
    {
        public float TimeToDestroy;

        public AutoDestroy(float timeToDestroy = 5)
        {
            TimeToDestroy = timeToDestroy;
        }
    }
}