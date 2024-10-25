using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VS.Runtime.Bootstrap.Units;
using VS.Runtime.Utilities;

namespace VS.Runtime.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneManager _sceneManager;

        public MetaFlow(LoadingService loadingService, SceneManager sceneManager)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(new FooLoadingUnit(3));
            _sceneManager.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}