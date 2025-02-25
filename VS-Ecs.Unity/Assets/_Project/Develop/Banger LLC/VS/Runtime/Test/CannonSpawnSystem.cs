using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Runtime.Core.Infrastructure;
using VS.Runtime.Core.Interfaces;
using VS.Runtime.Services.Grid;

namespace VS.Runtime.Test
{
    public class CannonSpawnSystem : IEcsInit
    {
        private readonly ResourcesContainer _resourcesContainer;
        private readonly EcsDefaultWorld _world;
        private Transform _cannonSpawnRoot;

        [Inject]
        public CannonSpawnSystem(EcsDefaultWorld world, ResourcesContainer resourcesContainer, ILevel level)
        {
            _cannonSpawnRoot = level.CannonSpawnRoot;
            _world = world;
            _resourcesContainer = resourcesContainer;
        }

        public void Init()
        {
            var cannon = Object.Instantiate(_resourcesContainer.Cannon, _cannonSpawnRoot);
            cannon.ConnectWith(_world.NewEntityLong(), true);
        }
    }
}