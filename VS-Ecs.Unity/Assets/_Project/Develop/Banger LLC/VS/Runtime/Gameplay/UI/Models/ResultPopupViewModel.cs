using R3;
using VContainer;
using VContainer.Unity;
using VS.Runtime.Services;
using VS.Runtime.Services.Session;

namespace VS.Runtime.Core.UI
{
    public class ResultPopupViewModel : ViewModel, IInitializable

    {
        private readonly ISceneService _sceneService;
        public ReadOnlyReactiveProperty<int> Score { get; }
        public ReactiveCommand Submit { get; } = new();

        [Inject]
        public ResultPopupViewModel(ISessionDataService sessionData, ISceneService sceneService)
        {
            Score = sessionData.Score;
            _sceneService = sceneService;
        }

        public void Initialize()
        {
            Submit.Subscribe(UnloadScene)
                .AddTo(Disposables);
        }

        //TODO: consider adding global events bus, for big things 
        private void UnloadScene(Unit _) => 
            _sceneService.UnloadSceneAsync(RuntimeConstants.Scenes.Core);
    }
}
