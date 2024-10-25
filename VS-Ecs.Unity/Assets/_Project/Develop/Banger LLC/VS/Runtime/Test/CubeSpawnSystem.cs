using Arch.Core;
using Arch.Unity;
using Arch.Unity.Toolkit;
using UnityEngine;
using VContainer;

namespace VS.Runtime.Test
{
    public class CubeSpawnSystem : UnitySystemBase
    {
        private ResourcesContainer _resourcesContainer;
        public CubeSpawnSystem(World world) : base(world) { }

        [Inject]
        private void Construct(ResourcesContainer resourcesContainer)
        {
            _resourcesContainer = resourcesContainer;
        }

        public override void Initialize()
        {
            base.Initialize();
            var cube = Object.Instantiate(_resourcesContainer.Cube);
            World.Create(cube, new RotationComponent());
        }
    }
}