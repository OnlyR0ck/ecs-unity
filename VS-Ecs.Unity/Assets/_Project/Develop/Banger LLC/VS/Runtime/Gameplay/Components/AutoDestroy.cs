using System;
using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct AutoDestroy : IEcsComponent
    {
        public float TimeToDestroy;

        public AutoDestroy(float timeToDestroy = 5)
        {
            TimeToDestroy = timeToDestroy;
        }
    }
    
    [Serializable]
    public class AutoDestroyComponentTemplate : ComponentTemplate<AutoDestroy>{} 
}