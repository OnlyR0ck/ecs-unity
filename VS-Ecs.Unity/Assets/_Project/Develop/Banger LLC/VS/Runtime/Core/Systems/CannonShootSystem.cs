using System.Collections.Generic;
using VS.Runtime.Services.Input;
using DCFApixels.DragonECS;
using UnityEngine;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Constants;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Extensions;
using VS.Runtime.Services.Grid;
using VS.Runtime.Test;
using VS.Runtime.Utilities.Debug;
using VS.Runtime.Core.Components.StateMachine;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace VS.Runtime.Core.Systems
{
    public sealed class CannonShootSystem : IEcsInit, IEcsDestroy
    {
        #region Nested

        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class CannonAspect : EcsAspect
        {
            public EcsPool<Cannon> Cannons = Inc;
        }

        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class ProjectileAspect : EcsAspect
        {
            public EcsPool<Bubble> Bubbles = Inc;
            public EcsPool<Path> Paths = Exc;
        }
        
        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class FlyingProjectileAspect : EcsAspect
        {
            public EcsPool<Bubble> Bubbles = Inc;
            public EcsPool<Path> Paths = Inc;
        }

        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class GridAspect : EcsAspect
        {
            public EcsPool<GridComponent> Grids = Inc;
        }

        #endregion


        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
        private readonly IInputService _inputService;
        private readonly BubbleView _bubblePrefab;
        private readonly ShootingConfig _config;
        private readonly EcsDefaultWorld _world;
        private readonly GridParams _params;
        private readonly GridModel _model;
        
        private Transform _bulletSpawnRoot;
        private Transform _aimLineRoot;
        private EBubbleColor _nextColor;

        public CannonShootSystem
        (
            EcsDefaultWorld world,
            IInputService inputService,
            ShootingConfig config,
            ResourcesContainer resourcesContainer,
            IGridParamsService paramsService,
            GridModel model
        )
        {
            _world = world;
            _config = config;
            _model = model;
            _inputService = inputService;
            _bubblePrefab = resourcesContainer.Bubble;
            _params = paramsService.Params;
        }


        public void Init()
        {
            CacheValues();
            _inputService.OnEndDrag += OnEndDrag_Handler;
            _nextColor = BubbleExtensions.GetRandomColor();
            Debug.Log($"Next color: {_nextColor}");
        }

        public void Destroy() => 
            _inputService.OnEndDrag -= OnEndDrag_Handler;

        private void CacheValues()
        {
            foreach (var entity in _world.Where(out CannonAspect aspect))
            {
                _bulletSpawnRoot = aspect.Cannons.Get(entity).BulletSpawnRoot;
                _aimLineRoot = aspect.Cannons.Get(entity).AimLineRoot;
            }
        }

        private void OnEndDrag_Handler(Vector2 vector2)
        {
            if (!IsAllowedToShoot())
                return;
            
            var projectile = Object.Instantiate(_bubblePrefab, _bulletSpawnRoot.position, Quaternion.identity);
            projectile.transform.localScale = _params.CellSize;
            projectile.SetColor(_nextColor);
            _nextColor = BubbleExtensions.GetRandomColor();
            Debug.Log($"Next color: {_nextColor}");
            
            entlong projectileEntity = _world.NewEntityLong();
            projectile.Connector.ConnectWith(projectileEntity, true);

            GetCollisionPoints(out var points, out Vector2Int? index);

            var pathPool = _world.GetPool<Path>();
            ref var path = ref pathPool.TryAddOrGet(projectileEntity.ID);
            path.Points = points.ToArray();
            
            if (!index.HasValue)
            {
                CustomDebugLog.LogError("The cell index wasn't found, something went wrong");
            }
            
            var replacementsPool = _world.GetPool<ProjectileToCell>();
            ref var replacement = ref replacementsPool.TryAddOrGet(projectileEntity.ID);
            replacement.Color = projectile.Color;
            replacement.Index = index ?? Vector2Int.zero;

        }

        private void GetCollisionPoints(out List<Vector3> collisionPoints, out Vector2Int? index)
        {
            Vector2 direction = _aimLineRoot.up; 
            Vector2 origin = _aimLineRoot.position;
            index = null;

            int maxReflections = 5;
            int currentReflection = 0;
            collisionPoints = new List<Vector3> { origin };
            

            while (currentReflection < maxReflections)
            {
                if (Physics2D.RaycastNonAlloc(origin, direction, _hits, _config.CastDistance, LayerMaskHash.Level) == 0)
                    break;
                
                var hit = _hits[0];
                if (hit.collider.TryGetComponent(out CellView view) && view.State == ECellState.Occupied)
                {
                    CellView cell = _model.FindClosestCell(hit.point, ECellState.Free);
                    Vector3 cellPosition = cell.transform.position;
                    index = cell.Coord;
                    collisionPoints.Add(hit.point);
                    collisionPoints.Add(cellPosition);
                    break;
                }

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
        
        private bool IsAllowedToShoot()
        {
            if (_world.GetPool<ShootingPhaseTag>().Count == 0)
                return false;

            foreach (var _ in _world.Where(out FlyingProjectileAspect _))
            {
                return false;
            }

            return true;
        }
    }
}