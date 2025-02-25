using DCFApixels.DragonECS;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Systems
{
    public class CleanUpSystem : IEcsRun
    {
        #if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class Aspect : EcsAspect
        {
            public EcsPool<AutoDestroy> Timers = Inc;
            public EcsPool<UnityComponent<Transform>> Transforms = Inc;
            
        }
        private readonly CommandBuffer _buffer = new();
        private readonly EcsDefaultWorld _world;

        [Inject]
        public CleanUpSystem(EcsDefaultWorld world)
        {
            _world = world;
        }
        
        public void Run()
        {
            float deltaTime = Time.deltaTime;
            foreach (int entity in _world.Where(out Aspect aspect))
            {
                ref AutoDestroy component = ref aspect.Timers.Get(entity);
                ref Transform transform = ref aspect.Transforms.Get(entity).obj;
                if (component.TimeToDestroy <= 0)
                {
                    //_world.DelEntity(entity);
                    Object.Destroy(transform.gameObject);
                    continue;
                }

                component.TimeToDestroy -= deltaTime;
            }
        }
    }
}