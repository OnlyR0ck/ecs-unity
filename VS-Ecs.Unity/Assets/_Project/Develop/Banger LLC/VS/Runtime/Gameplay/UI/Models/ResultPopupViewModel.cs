using Cysharp.Threading.Tasks;
using R3;
using VContainer;
using VS.Runtime.Services;
using VS.Runtime.Services.Session;

namespace VS.Runtime.Core.UI
{
    public class ResultPopupViewModel
    {
        public ReadOnlyReactiveProperty<int> Score { get; }
        public ReactiveCommand Submit { get; } = new();

        [Inject]
        public ResultPopupViewModel(ISessionDataService sessionData, ISceneService sceneService)
        {
            Score = sessionData.Score;
            
            //TODO: clear subscription?
            Submit.Subscribe(_ => sceneService.LoadScene(RuntimeConstants.Scenes.Meta).Forget());
        }
    }
}
