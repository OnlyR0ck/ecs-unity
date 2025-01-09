using DCFApixels.DragonECS;
using UnityEngine;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Views;
using VS.Runtime.Test;

namespace VS.Runtime.Core.Systems
{
    public class SpawnFieldSystem : IEcsInit
    {
        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class GridAspect : EcsAspect
        {
            public EcsPool<GridComponent> Grids = Inc;
        }

        private readonly EcsDefaultWorld _world;
        private readonly BubbleView _bubblePrefab;

        public SpawnFieldSystem(EcsDefaultWorld world, ResourcesContainer container)
        {
            _world = world;
            _bubblePrefab = container.BubbleCell;
        }

        public void Init()
        {
            SpawnContent();
        }

        private void SpawnContent()
        {
            foreach (int entity in _world.Where(out GridAspect aspect))
            {
                ref GridComponent grid = ref aspect.Grids.Get(entity);
                foreach (CellView cell in grid.Grid)
                {
                    BubbleView bubbleView = Object.Instantiate(_bubblePrefab, cell.transform, false);
                    cell.SetState(ECellState.Occupied);
                    bubbleView.SetColor(BubbleExtensions.GetRandomColor());
                }
            }
        }
    }
}