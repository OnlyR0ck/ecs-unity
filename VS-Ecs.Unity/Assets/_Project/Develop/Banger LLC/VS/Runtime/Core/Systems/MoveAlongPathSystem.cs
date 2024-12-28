using DCFApixels.DragonECS;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Systems
{
    public class MoveAlongPathSystem : IEcsInit, IEcsRun
    {
        private const float _defaultSpeed = 2.5f;
        private EcsDefaultWorld _world;

        public MoveAlongPathSystem(EcsDefaultWorld world)
        {
            _world = world;
        }

        public void Initialize()
        {
            
        }

        public void Init()
        {
            
        }

        public void Run()
        {
            
        }

        /*public void MoveAlongPath(float dt, ref PathComponent path, ref TransformComponent transformComponent)
        {
            /*if (transformComponent.Transform.position == path.Points[^1] || path.CurrentIndex >= path.Points.Length - 1)
            {
                _world.(path);
                return;
            }

            var direction = (path.Points[path.CurrentIndex + 1] - path.Points[path.CurrentIndex]).normalized;
            transformComponent.Transform.position += direction * _defaultSpeed * dt;
            if ((transformComponent.Transform.position - path.Points[path.CurrentIndex]).sqrMagnitude < Mathf.Epsilon)
                path.CurrentIndex++;#1#
        }*/
    }
}