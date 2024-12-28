using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;

namespace VS.Runtime.Test
{
    public class CannonSpawnSystem : IEcsInit
    {
        private readonly ResourcesContainer _resourcesContainer;
        private readonly EcsDefaultWorld _world;
        
        [Inject]
        public CannonSpawnSystem(EcsDefaultWorld world, ResourcesContainer resourcesContainer)
        {
            _world = world;
            _resourcesContainer = resourcesContainer;
        }

        public void Init()
        {
            var cannon = Object.Instantiate(_resourcesContainer.Cannon);
            cannon.ConnectWith(_world.NewEntityLong(), true);
        }
    }
}