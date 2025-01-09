using DCFApixels.DragonECS;
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
            public EcsPool<PathComponent> Objects = Inc;
            public EcsPool<UnityComponent<Transform>> Transforms = Inc;
        }
        
        private const float DefaultSpeed = 5f;
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
                ref PathComponent path = ref aspect.Objects.Get(entity);
                
                if (transform.position == path.Points[^1] || path.CurrentIndex >= path.Points.Length - 1)
                {
                    aspect.Objects.Del(entity);
                    return;
                }

                float dt = Time.deltaTime;
                var direction = (path.Points[path.CurrentIndex + 1] - path.Points[path.CurrentIndex]).normalized;
                transform.position += direction * DefaultSpeed * dt;
                if ((transform.position - path.Points[path.CurrentIndex + 1]).sqrMagnitude < Epsilon) 
                    path.CurrentIndex++;
            }
        }

        private const float Epsilon = 0.01f;
    }
}