using DCFApixels.DragonECS;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;
using VS.Runtime.Core.Components;
using VS.Runtime.Utilities.Debug;
using VS.Runtime.Utilities.Logging;

namespace VS.Runtime.Core.Systems
{
    public class MoveAlongPathSystem : IEcsRun
    {
        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class Aspect : EcsAspect
        {
            public EcsPool<Path> Objects = Inc;
            public EcsPool<UnityComponent<Transform>> Transforms = Inc;
        }
        
        private const float DefaultSpeed = 10f;
        private readonly EcsDefaultWorld _world;

        [Inject]
        public MoveAlongPathSystem(EcsDefaultWorld world)
        {
            _world = world;
        }

        public void Run()
        {
            foreach (int entity in _world.Where(out Aspect aspect))
            {
                Transform transform = aspect.Transforms.Get(entity).obj;
                ref Path path = ref aspect.Objects.Get(entity);

                if (path.CurrentIndex >= path.Points.Length - 1)
                {
                    transform.position = path.Points[^1];
                    aspect.Objects.Del(entity);
                    continue;
                }

                float dt = Time.deltaTime;
                var direction = (path.Points[path.CurrentIndex + 1] - path.Points[path.CurrentIndex]).normalized;
                transform.position += direction * DefaultSpeed * dt;

                // V30: dot-product detects passage even when projectile overshoots by > sqrt(epsilon)
                var toNext = path.Points[path.CurrentIndex + 1] - transform.position;
                if (Vector3.Dot(toNext, direction) <= 0)
                    path.CurrentIndex++;
            }
        }
    }
}