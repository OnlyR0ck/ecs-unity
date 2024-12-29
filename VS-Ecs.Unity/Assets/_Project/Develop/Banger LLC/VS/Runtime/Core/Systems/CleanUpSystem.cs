using DCFApixels.DragonECS;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;
using VS.Pool;
using VS.Pool.Interfaces;
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
            public EcsPool<AutoDestroyComponent> Timers = Inc;
            public EcsPool<GameObjectConnect> Connects = Inc;
            
        }
        private readonly IPool _pool;
        private readonly CommandBuffer _buffer = new();
        private readonly EcsDefaultWorld _world;

        [Inject]
        public CleanUpSystem(EcsDefaultWorld world, IPoolContainer poolContainer)
        {
            _world = world;
            _pool = poolContainer.GetPool(PoolsID.Level);
        }
        
        public void Run()
        {
            float deltaTime = Time.deltaTime;
            foreach (int entity in _world.Where(out Aspect aspect))
            {
                ref AutoDestroyComponent component = ref aspect.Timers.Get(entity);
                if (component.TimeToDestroy <= 0)
                {
                    _world.DelEntity(entity);
                }

                component.TimeToDestroy -= deltaTime;
            }
        }
    }
}