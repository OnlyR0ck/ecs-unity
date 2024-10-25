using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VS.Runtime.Bootstrap.Units;
using VS.Runtime.Utilities;

namespace VS.Runtime.Loading
{
    public class LoadingFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;

        public LoadingFlow(LoadingService loadingService, SceneManager sceneManager)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
        }

        public async void Start()
        {
            _sceneManager.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}