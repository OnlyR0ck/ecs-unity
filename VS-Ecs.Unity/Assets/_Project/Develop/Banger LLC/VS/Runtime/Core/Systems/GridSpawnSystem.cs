using VS.Runtime.Services.Grid;
using DCFApixels.DragonECS;
using UnityEngine;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Infrastructure;
using VS.Runtime.Core.Interfaces;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Test;

namespace VS.Runtime.Core.Systems
{
    public sealed class GridSpawnSystem : IEcsInit
    {
        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class GridAspect : EcsAspect
        {
            public EcsPool<GridComponent> Grids = Inc;
        }
        
        private readonly Transform _gridSpawnRoot;
        private readonly EcsDefaultWorld _world;
        private readonly Camera _camera;
        private readonly GridView _gridViewPrefab;
        private readonly GridParams _params;
        private GridModel _gridModel;

        public GridSpawnSystem(EcsDefaultWorld world, ILevel level, ICoreGameSceneRefs refs, ResourcesContainer resourcesContainer, IGridParamsService paramsService, GridModel gridModel)
        {
            _gridModel = gridModel;
            _world = world;
            _gridSpawnRoot = level.GridSpawnRoot;
            _camera = refs.GameCamera;
            _gridViewPrefab = resourcesContainer.GridViewPrefab;
            _params = paramsService.Params;
        }
        
        public void Init() => SpawnGrid();

        private void SpawnGrid()
        {
            GridView gridView = Object.Instantiate(_gridViewPrefab, _gridSpawnRoot);
            gridView.Connector.ConnectWith(_world.NewEntityLong(), true);

            float height = _camera.orthographicSize * 2;
            
            CellView[,] views = new CellView[_params.Config.Rows, _params.Config.Columns];
            Vector2 cellSize = _params.CellSize;
            Vector2 startOffset = cellSize * _params.Config.StartOffset;
            
            bool isEven = _params.Config.StartIsEven;
            Vector3 origin = _camera.ScreenToWorldPoint(Vector3.zero) +
                             new Vector3(startOffset.x * _params.Config.StartOffset, height - startOffset.y);
            
            for (int i = 0; i < _params.Config.Rows; i++)
            {
                float offsetX = isEven ? cellSize.x / 2 : 0;
                for (int j = 0; j < _params.Config.Columns; j++)
                {
                    Vector2 position = origin + new Vector3(
                        offsetX + j * cellSize.x,
                        -i * cellSize.y
                    );
                    
                    CellView cell = Object.Instantiate(_params.Config.CellViewPrefab, position, Quaternion.identity);
                    cell.SetCoord(new Vector2Int(i, j));
                    ECellState cellState = i < _params.Config.VisibleRows ? ECellState.Free : ECellState.NotAvailable;
                    cell.SetState(cellState);
                    cell.transform.localScale = cellSize;
                    cell.transform.SetParent(gridView.GridSpawnRoot);
                    views[i, j] = cell;
                }

                isEven = !isEven;
            }
            
            foreach (int entity in _world.Where(out GridAspect aspect))
            {
                ref GridComponent grid = ref aspect.Grids.Get(entity);
                grid.Cells = views;
                grid.StartIsEven = _params.Config.StartIsEven;
                _gridModel.Initialize(ref grid);
            }
        }
    }
}