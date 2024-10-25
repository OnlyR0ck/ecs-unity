using Arch.Core;
using Arch.Core.Extensions;
using Arch.Unity.Conversion;
using Arch.Unity.Toolkit;
using UnityEngine;

namespace VS.Runtime.Test
{
    public sealed class RotationSystem : UnitySystemBase
    {
        private readonly World _world;
    
        public RotationSystem(World world) : base(world)
        {
            _world = world;
        }

        public override void Update(in SystemState t)
        {
            base.Update(in t);
            float dt = t.DeltaTime;
            var query = new QueryDescription().WithAll<RotationComponent>();

            _world.Query(in query, (Entity entity, ref RotationComponent rot) =>
            {
                // Update the rotation based on speed
                entity.Get<RotationComponent>();
                rot.Rotation *= Quaternion.Euler(0, rot.Speed * dt, 0);
                
                if (EntityConversion.TryGetGameObject(entity, out GameObject go))
                {
                    go.transform.rotation = rot.Rotation;
                }
            });
        }
    }
}