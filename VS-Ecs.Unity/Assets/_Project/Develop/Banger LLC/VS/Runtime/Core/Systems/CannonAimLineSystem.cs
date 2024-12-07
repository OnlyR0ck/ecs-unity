using System.Collections.Generic;
using Arch.Core;
using Arch.Unity.Toolkit;
using UnityEngine;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Constants;
using VS.Runtime.Test;
using VS.Runtime.Utilities;
using Object = UnityEngine.Object;

namespace VS.Runtime.Core.Systems
{
    public class CannonAimLineSystem : UnitySystemBase
    {
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
        private readonly IInputService _inputService;
        private readonly LineRenderer _aimLinePrefab;
        private readonly ShootingConfig _config;
        private LineRenderer _aimLineRenderer;
        private Transform _aimLineRoot;
        private float _deltaTime;


        public CannonAimLineSystem(World world, IInputService inputService, ShootingConfig config, ResourcesContainer resources) : base(world)
        {
            _config = config;
            _inputService = inputService;
            _aimLinePrefab = resources.AimLine;
        }

        public override void Initialize()
        {
            _inputService.OnStartDrag += OnStartDrag_Handler;
            _inputService.OnEndDrag += OnEndDrag_Handler;
            _inputService.OnDrag += OnDrag_Handler;
            
            CacheValues();
            SpawnAimLine();
            
            _aimLineRoot.gameObject.SetActive(false);
        }

        public override void Dispose()
        {
            _inputService.OnStartDrag -= OnStartDrag_Handler;
            _inputService.OnEndDrag -= OnEndDrag_Handler;
            _inputService.OnDrag -= OnDrag_Handler;
        }

        public override void Update(in SystemState state)
        {
            base.Update(in state);
            _deltaTime = state.DeltaTime;
            RefreshLineRenderer(_deltaTime);
        }

        private void SpawnAimLine() => 
            _aimLineRenderer = Object.Instantiate(_aimLinePrefab, _aimLineRoot, false);

        private void CacheValues()
        {
            var query = new QueryDescription().WithAll<Cannon>();
            World.Query(in query, (ref Cannon cannon) =>
            {
                _aimLineRoot = cannon.AimLineRoot;
            });
        }

        private void RefreshLineRenderer(float deltaTime)
        {
            float yOffset = Mathf.Repeat(deltaTime * 0.5f, 1f);
            _aimLineRenderer.material.SetTextureOffset(MainTex, new Vector2(1, yOffset));
        }
        
        private void RecalculateAimLineDotsPositions()
        {
            Vector2 direction = _aimLineRoot.up; 
            Vector2 origin = _aimLineRoot.position;

            int maxReflections = 5;
            List<Vector3> collisionPoints = new List<Vector3>();
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