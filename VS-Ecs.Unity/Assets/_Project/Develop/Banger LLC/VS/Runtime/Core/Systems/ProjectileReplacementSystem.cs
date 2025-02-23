using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Test;

namespace VS.Runtime.Core.Systems
{
    public class ProjectileReplacementSystem : IEcsRun
    {
        private readonly EcsDefaultWorld _world;
        private readonly GridModel _model;
        private readonly BubbleView _cellPrefab;

        #region Nested

        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class ProjectileAspect : EcsAspect
        {
            public EcsPool<Bubble> Bubbles = Inc;
            public EcsPool<Path> Paths = Exc;
            public EcsPool<ProjectileToCell> ProjectileToCells = Inc;
            public EcsPool<UnityComponent<Transform>> Transforms = Inc;
        }

        #endregion

        [Inject]
        public ProjectileReplacementSystem(EcsDefaultWorld world, GridModel model, ResourcesContainer container)
        {
            _model = model;
            _world = world;
            _cellPrefab = container.BubbleCell;
        }
        
        public void Run()
        {
            foreach (var entity in _world.Where(out ProjectileAspect aspect))
            {
                ref var projectileToCell = ref aspect.ProjectileToCells.Get(entity);
                Vector2Int index = projectileToCell.Index;
                CellView cell = _model.Grid.Cells[index.x, index.y];

                var bubbleView = Object.Instantiate(_cellPrefab, cell.transform);
                bubbleView.SetColor(projectileToCell.Color);
                cell.SetState(ECellState.Occupied);
                
                ref var transform = ref aspect.Transforms.Get(entity);
                Object.Destroy(transform.obj.gameObject);
            }
        }
    }
}