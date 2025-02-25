using UnityEngine;
using VContainer.Unity;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Infrastructure;

namespace VS.Runtime.Services.Grid
{
    public sealed class GridParamsService :  IInitializable, IGridParamsService
    {
        private readonly GridSettingsConfig _config;
        private readonly Camera _camera;

        public GridParams Params { get; private set; }

        public GridParamsService(GridSettingsConfig config, ICoreGameSceneRefs refs)
        {
            _config = config;
            _camera = refs.GameCamera;
        }

        public void Initialize()
        {
            Vector2 cellSize = CalculateCellSize();
            Params = new GridParams(cellSize, _config);
        }

        private Vector2 CalculateCellSize()
        {
            float height = _camera.orthographicSize * 2;
            float width  = height * _camera.aspect;
            Vector2 screenWorldSize = new Vector2(width, height);
            
            float cellRadius = screenWorldSize.x / (_config.Columns + _config.OffsetColumns);
            return new Vector2(cellRadius, cellRadius);
        }
    }

    public sealed class GridParams
    {
        public Vector2 CellSize { get; }
        public GridSettingsConfig Config { get; }

        public GridParams(Vector2 cellSize, GridSettingsConfig config)
        {
            CellSize = cellSize;
            Config = config;
        }
    }
}