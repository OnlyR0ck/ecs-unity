using VS.Runtime.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VS.Runtime.Bootstrap.Units;

namespace VS.Runtime.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly ILoadingService _loadingService;
        private readonly ISceneService _sceneService;

        public MetaFlow(ILoadingService loadingService, ISceneService sceneService)
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