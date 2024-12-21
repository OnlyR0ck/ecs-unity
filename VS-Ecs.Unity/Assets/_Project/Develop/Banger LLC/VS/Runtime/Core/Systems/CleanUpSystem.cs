using System.Collections.Generic;
using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Arch.Unity.Conversion;
using Arch.Unity.Toolkit;
using UnityEngine;
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
        private readonly CommandBuffer _buffer = new(); 

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
            var query = new QueryDescription().WithAll<AutoDestroyComponent>();
            var state = t;
            World.Query(in query, (ref Entity entity, ref AutoDestroyComponent component) =>
            {
                component.TimeToDestroy -= state.DeltaTime;
                if (component.TimeToDestroy <= 0)
                {
                    _buffer.Destroy(entity);
                    //World.Destroy(entity);
                } 
            });
            
            
            _buffer.Playback(World);
        }

        /*[Query]
        public void CleanUp([Data] in float dt, in Entity entity, ref AutoDestroyComponent component)
        {
            component.TimeToDestroy -= dt;
            if (component.TimeToDestroy <= 0)
            {
                World.Destroy(entity);
            }
        }*/

    }
}