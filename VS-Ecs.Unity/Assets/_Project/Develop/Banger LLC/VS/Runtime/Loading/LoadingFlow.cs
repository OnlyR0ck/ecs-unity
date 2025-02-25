using VS.Runtime.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VS.Runtime.Bootstrap.Units;
using VS.Runtime.Utilities;

namespace VS.Runtime.Loading
{
    public class LoadingFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneService _sceneService;

        public LoadingFlow(LoadingService loadingService, SceneService sceneService)
        {
            _loadingService = loadingService;
            _sceneService = sceneService;
        }

        public async void Start()
        {
            _sceneService.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}