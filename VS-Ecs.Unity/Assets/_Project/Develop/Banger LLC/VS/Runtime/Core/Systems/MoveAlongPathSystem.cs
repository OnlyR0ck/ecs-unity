using System.Collections.Generic;
using Arch.Core;
using Arch.System;
using Arch.Unity.Toolkit;
using UnityEngine;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Systems
{
    public class MoveAlongPathSystem : UnitySystemBase
    {
        private const float _defaultSpeed = 2.5f;
        private List<(Entity Entity, PathComponent Path)> _toMove;

        public MoveAlongPathSystem(World world) : base(world) { }

        public override void Initialize()
        {
            base.Initialize();
            World.SubscribeComponentAdded((in Entity entity, ref PathComponent path) =>
            {
                _toMove.Add((entity, path));
            });
        }

        [Query]
        public void MoveAlongPath([Data] in float dt,in Entity entity, ref PathComponent path, ref TransformComponent transformComponent)
        {
            if (transformComponent.Transform.position == path.Points[^1] || path.CurrentIndex >= path.Points.Length - 1)
            {
                World.Remove<PathComponent>(entity);
                return;
            }

            var direction = (path.Points[path.CurrentIndex + 1] - path.Points[path.CurrentIndex]).normalized;
            transformComponent.Transform.position += direction * _defaultSpeed * dt;
            if ((transformComponent.Transform.position - path.Points[path.CurrentIndex]).sqrMagnitude < Mathf.Epsilon)
                path.CurrentIndex++;
        }
    }
}