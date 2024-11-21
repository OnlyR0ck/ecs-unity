using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VS.Runtime.Bootstrap.Units;
using VS.Runtime.Utilities;

namespace VS.Runtime.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneService _sceneService;

        public MetaFlow(LoadingService loadingService, SceneService sceneService)
        {
            _loadingService = loadingService;
            _sceneService = sceneService;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(new FooLoadingUnit(3));
            _sceneService.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}