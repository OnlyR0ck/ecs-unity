using Arch.Core;
using Arch.Core.Utils;
using VContainer.Unity;
using VS.Runtime.Utilities;

namespace VS.Runtime.Core
{
    public class CoreFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;

        public CoreFlow(LoadingService loadingService, SceneManager sceneManager)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
        }

        public async void Start()
        {
            World world = World.Create();
            world.Create();
        }
    }
}