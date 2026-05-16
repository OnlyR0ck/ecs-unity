using VS.Runtime.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace VS.Runtime.Loading
{
    public class LoadingFlow : IStartable
    {
        private readonly ISceneService _sceneService;

        public LoadingFlow(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }

        public async void Start()
        {
            _sceneService.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}