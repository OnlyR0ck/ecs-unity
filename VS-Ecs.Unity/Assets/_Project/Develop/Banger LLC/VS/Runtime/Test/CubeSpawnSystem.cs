using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;

namespace VS.Runtime.Test
{
    public class CubeSpawnSystem : IEcsInit
    {
        private ResourcesContainer _resourcesContainer;
        private readonly EcsDefaultWorld _world;
        public CubeSpawnSystem(EcsDefaultWorld world)
        {
            _world = world;
        }

        [Inject]
        private void Construct(ResourcesContainer resourcesContainer)
        {
            _resourcesContainer = resourcesContainer;
        }

        public void Init()
        {
            var cube = Object.Instantiate(_resourcesContainer.Cube);
            _world.Add(_resourcesContainer.Cube);
        }
    }
}