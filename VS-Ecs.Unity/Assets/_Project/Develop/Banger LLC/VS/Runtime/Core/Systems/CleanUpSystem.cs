using DCFApixels.DragonECS;
using UnityEngine.Rendering;
using VS.Pool;
using VS.Pool.Interfaces;

namespace VS.Runtime.Core.Systems
{
    public class CleanUpSystem : IEcsRun
    {
        private readonly IPool _pool;
        private readonly CommandBuffer _buffer = new();
        private EcsDefaultWorld _world;

        public CleanUpSystem(EcsDefaultWorld world, IPoolContainer poolContainer)
        {
            _world = world;
            _pool = poolContainer.GetPool(PoolsID.Level);
        }
        
        public void Run()
        {
            /*var query = new QueryDescription().WithAll<AutoDestroyComponent>();
            var state = t;
            World.Query(in query, (ref Entity entity, ref AutoDestroyComponent component) =>
            {
                component.TimeToDestroy -= state.DeltaTime;
                if (component.TimeToDestroy <= 0)
                {
                    World.Destroy(entity);
                } 
            });*/
        }
    }
}