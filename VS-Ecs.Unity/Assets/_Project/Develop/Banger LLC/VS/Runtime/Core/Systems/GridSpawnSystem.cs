using DCFApixels.DragonECS;
using UnityEngine;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Infrastructure;
using VS.Runtime.Core.Interfaces;
using VS.Runtime.Core.Views;
using VS.Runtime.Test;

namespace VS.Runtime.Core.Systems
{
    public class GridSpawnSystem : IEcsInit
    {
        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class GridAspect : EcsAspect
        {
            public EcsPool<UnityComponent<Transform>> Transforms = Inc;
            public EcsPool<GridComponent> Grids = Opt;
        }
        
        private readonly Transform _gridSpawnRoot;
        private readonly GridSettingsConfig _config;
        private readonly EcsDefaultWorld _world;
        private readonly Camera _camera;
        private readonly GridView _gridViewPrefab;

        public GridSpawnSystem(EcsDefaultWorld world, ILevel level, ICoreGameSceneRefs refs, GridSettingsConfig config, ResourcesContainer resourcesContainer)
        {
            _world = world;
            _config = config;
            _gridSpawnRoot = level.GridSpawnRoot;
            _camera = refs.GameCamera;
            _gridViewPrefab = resourcesContainer.GridViewPrefab;
        }
        
        public void Init()
        {
            SpawnGrid();
        }

        private void SpawnGrid()
        {
            GridView gridView = Object.Instantiate(_gridViewPrefab, _gridSpawnRoot);
            gridView.Connector.ConnectWith(_world.NewEntityLong(), true);

            float height = _camera.orthographicSize * 2;
            float width  = height * _camera.aspect;
            Vector2 screenWorldSize = new Vector2(width, height);
            
            CellView[,] views = new CellView[_config.Rows, _config.Columns];
            float cellRadius = screenWorldSize.x / (_config.Columns + _config.OffsetColumns);
            Vector2 cellSize = new Vector2(cellRadius, cellRadius);
            
            bool isEven = _config.StartIsEven;
            Vector3 origin = _camera.ScreenToWorldPoint(Vector3.zero) + new Vector3(cellSize.x, height - cellSize.y);

            
            for (int i = 0; i < _config.Rows; i++)
            {
                float offsetX = isEven ? cellSize.x / 2 : 0;
                for (int j = 0; j < _config.Columns; j++)
                {
                    Vector2 position = origin + new Vector3(
                        offsetX + j * cellSize.x,
                        -i * cellSize.y
                    );
                    
                    CellView cell = Object.Instantiate(_config.CellViewPrefab, position, Quaternion.identity);
                    cell.SetCoord(new Vector2Int(i, j));
                    cell.SetState(ECellState.Free);
                    cell.transform.localScale = cellSize;
                    cell.transform.SetParent(gridView.GridSpawnRoot);
                    views[i, j] = cell;
                }

                isEven = !isEven;
            }
            
            foreach (int entity in _world.Where(out GridAspect aspect))
            {
                ref GridComponent grid = ref aspect.Grids.TryAddOrGet(entity);
                grid.Grid = views;
            }
        }
    }
}