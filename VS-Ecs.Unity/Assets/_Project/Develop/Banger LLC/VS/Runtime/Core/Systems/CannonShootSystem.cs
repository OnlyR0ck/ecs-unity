using System.Collections.Generic;
using DCFApixels.DragonECS;
using UnityEngine;
using VS.Core.Configs.Features;
using VS.Pool;
using VS.Pool.Interfaces;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Constants;
using VS.Runtime.Core.Views;
using VS.Runtime.Test;
using VS.Runtime.Utilities;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace VS.Runtime.Core.Systems
{
    public class CannonShootSystem : IEcsInit, IEcsDestroy
    {
        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class Aspect : EcsAspect
        {
            public EcsPool<CannonComponent> Cannons = Inc;
        }

        #if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class ProjectileAspect : EcsAspect
        {
            public EcsPool<BubbleComponent> Bubbles = Inc;
            public EcsPool<PathComponent> Paths = Opt;
            public EcsPool<AutoDestroyComponent> AutoDestroyComponents = Opt;
        }
        
        private readonly IInputService _inputService;
        private Transform _bulletSpawnRoot;
        private readonly IPool _pool;
        private Transform _aimLineRoot;
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
        private readonly ShootingConfig _config;
        private readonly EcsEntityConnect _bubblePrefab;
        private readonly EcsDefaultWorld _world;

        public CannonShootSystem(EcsDefaultWorld world, IInputService inputService, ShootingConfig config, IPoolContainer container, ResourcesContainer resourcesContainer)
        {
            _world = world;
            _config = config;
            _inputService = inputService;
            _pool = container.GetPool(PoolsID.Level);
            _bubblePrefab = resourcesContainer.Bubble;
        }
        

        public void Init()
        {
            CacheValues();
            _inputService.OnEndDrag += OnEndDrag_Handler;
        }

        public void Destroy() => 
            _inputService.OnEndDrag -= OnEndDrag_Handler;

        private void CacheValues()
        {
            foreach (var entity in _world.Where(out Aspect aspect))
            {
                _bulletSpawnRoot = aspect.Cannons.Get(entity).BulletSpawnRoot;
                _aimLineRoot = aspect.Cannons.Get(entity).AimLineRoot;
            }
        }

        private void OnEndDrag_Handler(Vector2 vector2)
        {
            if (!IsAllowedToShoot())
                return;

            //var view = _pool.Get<BubbleView>(LevelPoolIDs.BubbleID, nulxl, _bulletSpawnRoot.position);
            var projectile = Object.Instantiate(_bubblePrefab, _bulletSpawnRoot.position, Quaternion.identity);
            entlong projectileEntity = _world.NewEntityLong();
            projectile.ConnectWith(projectileEntity, true);

            GetCollisionPoints(out var points);

            foreach (int entity in _world.Where(out ProjectileAspect aspect))
            {
                ref PathComponent path = ref aspect.Paths.TryAddOrGet(entity);
                path.Points = points.ToArray();

                ref AutoDestroyComponent autoDestroy = ref aspect.AutoDestroyComponents.TryAddOrGet(entity);
                autoDestroy.TimeToDestroy = 1;
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