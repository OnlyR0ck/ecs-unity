using System.Collections.Generic;
using DCFApixels.DragonECS;
using UnityEngine;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Constants;
using VS.Runtime.Test;
using VS.Runtime.Utilities;
using Object = UnityEngine.Object;

namespace VS.Runtime.Core.Systems
{
    public sealed class CannonAimLineSystem : IEcsInit, IEcsDestroy, IEcsRun
    {
        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private sealed class CannonAspect : EcsAspect
        {
            public EcsPool<CannonComponent> Cannons = Inc;
        }

        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class AimLineAspect : EcsAspect
        {
            public EcsPool<AimLineComponent> AimLines = Inc;
        }
        
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
        private readonly IInputService _inputService;
        private readonly EcsEntityConnect _aimLinePrefab;
        private readonly ShootingConfig _config;
        private LineRenderer _aimLineRenderer;
        private Transform _aimLineRoot;
        private float _deltaTime;
        private readonly EcsDefaultWorld _world;

        public CannonAimLineSystem(EcsDefaultWorld world, IInputService inputService, ShootingConfig config, ResourcesContainer resources)
        {
            _world = world;
            _config = config;
            _inputService = inputService;
            _aimLinePrefab = resources.AimLine;
        }

        public void Init()
        {
            _inputService.OnStartDrag += OnStartDrag_Handler;
            _inputService.OnEndDrag += OnEndDrag_Handler;
            _inputService.OnDrag += OnDrag_Handler;

            CacheValues();
            SpawnAimLine();

            _aimLineRoot.gameObject.SetActive(false);
        }

        public void Destroy()
        {
            _inputService.OnStartDrag -= OnStartDrag_Handler;
            _inputService.OnEndDrag -= OnEndDrag_Handler;
            _inputService.OnDrag -= OnDrag_Handler;
        }

        public void Run()
        {
            _deltaTime = Time.deltaTime;
            AnimateLineRenderer(_deltaTime);
        }

        private void SpawnAimLine()
        {
            EcsEntityConnect connect = Object.Instantiate(_aimLinePrefab, _aimLineRoot, false);
            entlong entity = _world.NewEntityLong();
            connect.ConnectWith(entity, true);
            _aimLineRenderer = connect.GetComponent<LineRenderer>();
        }

        private void CacheValues()
        {
            int cannonEntity = _world.Where(out CannonAspect cannonAspect).First();
            _aimLineRoot = cannonAspect.Cannons.Get(cannonEntity).AimLineRoot;
        }

        private void AnimateLineRenderer(float deltaTime)
        {
            float yOffset = Mathf.Repeat(deltaTime * 0.5f, 1f);
            _aimLineRenderer.material.SetTextureOffset(MainTex, new Vector2(1, yOffset));
        }

        private void RecalculateAimLineDotsPositions()
        {
            Vector2 direction = _aimLineRoot.up; 
            Vector2 origin = _aimLineRoot.position;

            int maxReflections = 5;
            List<Vector3> collisionPoints = new List<Vector3> { origin };

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

            _aimLineRenderer.positionCount = collisionPoints.Count;
            _aimLineRenderer.SetPositions(collisionPoints.ToArray());
        }


        private void SetAimLineActive(bool isActive) =>
            _aimLineRoot.gameObject.SetActive(isActive);

        private void OnDrag_Handler(Vector2 _) => 
            RecalculateAimLineDotsPositions();

        private void OnStartDrag_Handler(Vector2 _) => 
            SetAimLineActive(true);

        private void OnEndDrag_Handler(Vector2 _) => 
            SetAimLineActive(false);
    }
}