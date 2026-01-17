using System.Collections.Generic;
using DCFApixels;
using DCFApixels.DragonECS;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Extensions;
using VS.Runtime.Test;

namespace VS.Runtime.Core.Systems
{
    public sealed class HighlightNeighborCellsSystem : IEcsRun, IEcsInit
    {
        #if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class Aspect : EcsAspect
        {
            public EcsPool<AutoDestroy> Pool = Exc;
            
        }

        private readonly GridModel _model;
        private EcsEntityConnect _highlightPrefab;
        private RaycastHit2D[] _hits;
        private Camera _camera;
        private LayerMask _layerMask;
        private EcsDefaultWorld _world;

        [Inject]
        public HighlightNeighborCellsSystem(EcsDefaultWorld world, ResourcesContainer resources, GridModel model)
        {
            _world = world;
            _model = model;
            _highlightPrefab = resources.CellHighlight;
        }

        public void Init()
        {
            _camera = Camera.main;
            _layerMask = LayerMask.GetMask("Level");
            _hits = new RaycastHit2D[10];
        }

        public void Run()
        {
            if (!Input.GetMouseButtonDown(0))
                return;
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
            if (Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _hits, _layerMask) == 0)
                return;

            if (!_hits[0].collider.TryGetComponent(out CellView view))
                return;

            
            Vector2Int index = _model.FindClosestCellIndex(view.transform.position);
            IReadOnlyCollection<Vector2Int> indices = _model.GetNeighbors(index);
            DebugX.Draw(Color.black).Text(_camera.WorldToScreenPoint(_hits[0].transform.position), index);
            foreach (Vector2Int i in indices)
            {
                var cell = _model[i];
                DebugX.Draw(Color.black).Text(_camera.WorldToScreenPoint(cell.transform.position), i);
                entlong entity = _world.NewEntityLong();
                var highlighter = Object.Instantiate(_highlightPrefab, cell.transform);
                highlighter.ConnectWith(entity, true);
            }
        }
    }
}