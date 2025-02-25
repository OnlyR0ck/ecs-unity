using VS.Runtime.Services.Input;
using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Constants;
using VS.Runtime.Utilities;

namespace VS.Runtime.Core.Systems
{
    public sealed class CannonRotationSystem : IEcsInit, IEcsDestroy 
    {
        private class Aspect : EcsAspect
        {
            public EcsPool<Cannon> Cannons = Inc;
            public EcsPool<UnityComponent<Transform>> Transforms = Inc;
            public EcsPool<Rotation> Rotations = Inc;
        }
        
        private readonly IInputService _inputService;
        private readonly EcsDefaultWorld _world;

        [Inject]
        public CannonRotationSystem(EcsDefaultWorld world, IInputService inputService)
        {
            _world = world;
            _inputService = inputService;
        }
        
        public void Init() =>
            _inputService.OnDrag += OnDrag_Handler;

        public void Destroy() => 
            _inputService.OnDrag -= OnDrag_Handler;

        private void OnDrag_Handler(Vector2 position)
        {
            foreach (var entity in _world.Where(out Aspect aspect))
            {
                var root = aspect.Transforms.Get(entity).obj;
                ref var rotation = ref aspect.Rotations.Get(entity);
                
                Vector3 direction = ((Vector3)position - root.position).normalized;
                direction.z = 0;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - Coordinates.AlignTo2DDegrees;
                root.localRotation = Quaternion.Euler(0, 0, angle);
                root.eulerAngles = ClampRotation(root, rotation.FromTo.x, rotation.FromTo.y);
            }
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