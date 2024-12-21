using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.Unity.Conversion;
using Arch.Unity.Toolkit;
using UnityEngine;
using UnityEngine.EventSystems;
using VS.Core.Configs.Features;
using VS.Pool;
using VS.Pool.Interfaces;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Constants;
using VS.Runtime.Core.Views;
using VS.Runtime.Test;
using VS.Runtime.Utilities;

namespace VS.Runtime.Core.Systems
{
    public class CannonShootSystem : UnitySystemBase
    {
        private readonly IInputService _inputService;
        private Transform _bulletSpawnRoot;
        private readonly IPool _pool;
        private Transform _aimLineRoot;
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
        private readonly ShootingConfig _config;
        private readonly BubbleView _bubblePrefab;

        public CannonShootSystem(World world, IInputService inputService, ShootingConfig config, IPoolContainer container, ResourcesContainer resourcesContainer) : base(world)
        {
            _config = config;
            _inputService = inputService;
            _pool = container.GetPool(PoolsID.Level);
            _bubblePrefab = resourcesContainer.Bubble;
        }

        public override void Initialize()
        {
            CacheValues();
            _inputService.OnEndDrag += OnEndDrag_Handler;
        }

        public override void Dispose() => 
            _inputService.OnEndDrag -= OnEndDrag_Handler;
        
        private void CacheValues()
        {
            var query = new QueryDescription().WithAll<CannonComponent>();
            World.Query(in query, (ref CannonComponent cannon) =>
            {
                _bulletSpawnRoot = cannon.BulletSpawnRoot;
                _aimLineRoot = cannon.AimLineRoot;
            });
        }

        private void OnEndDrag_Handler(Vector2 vector2)
        {
            if (!IsAllowedToShoot())
                return;
            
            //var view = _pool.Get<BubbleView>(LevelPoolIDs.BubbleID, nulxl, _bulletSpawnRoot.position);
            var view = Object.Instantiate(_bubblePrefab, _bulletSpawnRoot.position, Quaternion.identity);
            GetCollisionPoints(out var points);
            if (EntityConversion.TryGetEntity(view.gameObject, out EntityReference entity))
            {
                World.Add(entity, new PathComponent(points.ToArray()), new AutoDestroyComponent());
            }
        }
        
        private void GetCollisionPoints(out List<Vector3> collisionPoints)
        {
            Vector2 direction = _aimLineRoot.up; 
            Vector2 origin = _aimLineRoot.position;

            int maxReflections = 5;
            collisionPoints = new List<Vector3>();
            collisionPoints.Add(origin);

            int currentReflection = 0;

            while (currentReflection < maxReflections)
            {
                if (Physics2D.RaycastNonAlloc(origin, direction, _hits, _config.CastDistance, LayerMaskHash.Level) == 0)
                    break;

                var hit = _hits[0];

                if (hit.normal == Vector2.down)
                {
                    collisionPoints.Add(hit.point);
                    break;
                }

                collisionPoints.Add(hit.point);

                direction = Vector2.Reflect(direction, hit.normal);
                origin = hit.point + _config.CastOffset * hit.normal;

                currentReflection++;
            }
        }

        private bool IsAllowedToShoot() => true;
    }
}