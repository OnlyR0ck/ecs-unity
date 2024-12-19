using System.Collections.Generic;
using Arch.Core;
using Arch.Unity.Conversion;
using Arch.Unity.Toolkit;
using VS.Pool;
using VS.Pool.Interfaces;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Core.Systems
{
    public class CleanUpSystem : UnitySystemBase
    {
        private readonly List<(Entity Entity, AutoDestroyComponent Component)> _toDestroy = new();
        private readonly IPool _pool;

        public CleanUpSystem(World world, IPoolContainer poolContainer) : base(world)
        {
            _pool = poolContainer.GetPool(PoolsID.Level);
        }

        public override void Initialize()
        {
            base.Initialize();
            World.SubscribeComponentAdded((in Entity entity, ref AutoDestroyComponent autoDestroy) =>
            {
                _toDestroy.Add((entity, autoDestroy));
            });
        }

        public override void Update(in SystemState t)
        {
            base.Update(in t);
            for (int i = _toDestroy.Count - 1; i >= 0; --i)
            {
                _toDestroy[i].Component.TimeToDestroy -= t.DeltaTime;
                if (_toDestroy[i].Component.TimeToDestroy > 0)
                    continue;

                if (EntityConversion.TryGetGameObject(_toDestroy[i].Entity, out var gameObject))
                {
                    if (gameObject.TryGetComponent(out PoolObject view))
                    {
                        _pool.Release(view);
                    }
                }

                World.Remove<Entity>(_toDestroy[i].Entity);
                _toDestroy.RemoveAt(i);
            }
        }
    }
}