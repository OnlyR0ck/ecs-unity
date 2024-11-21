using System.Threading;
using Arch.Core;
using Arch.Unity.Toolkit;
using UnityEngine;
using VContainer;
using VS.Runtime.Core.Components;
using VS.Runtime.Utilities;

namespace VS.Runtime.Core.Systems
{
    public sealed class CannonRotationSystem : UnitySystemBase
    {
        private readonly IInputService _inputService;
        private CancellationTokenSource _cts;
        private const float AlignTo2DDegrees = 90;
        private float _deltaTime;

        [Inject]
        public CannonRotationSystem(World world, IInputService inputService) : base(world)
        {
            _inputService = inputService;
        }

        public override void Initialize()
        {
            _inputService.OnDrag += OnDrag_Handler;
        }

        public override void Dispose()
        {
            _inputService.OnDrag -= OnDrag_Handler;
        }

        public override void Update(in SystemState state)
        {
            base.Update(in state);
            _deltaTime = state.DeltaTime;
        }

        private void OnDrag_Handler(Vector2 position)
        {
            var query = new QueryDescription().WithAll<Cannon, RotationComponent, TransformComponent>();
            
            World.Query(in query, (ref Cannon _, ref RotationComponent rotation, ref TransformComponent root) =>
            {
                Vector3 direction = ((Vector3)position - root.Transform.position).normalized;
                direction.z = 0;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - AlignTo2DDegrees;
                root.Transform.localRotation = Quaternion.Euler(0, 0, angle);
                root.Transform.eulerAngles = ClampRotation(root.Transform, rotation.FromTo.x, rotation.FromTo.y);
            });
        }

        private Vector3 ClampRotation(Transform root, float minAngle, float maxAngle)
        {
            Vector3 rotation = root.localRotation.eulerAngles;
            if (rotation.z < 360 + minAngle && rotation.z > maxAngle)
                return rotation.z > 180 ? new Vector3(0, 0, minAngle) : new Vector3(0, 0, maxAngle);
            return rotation;

        }
    }
}