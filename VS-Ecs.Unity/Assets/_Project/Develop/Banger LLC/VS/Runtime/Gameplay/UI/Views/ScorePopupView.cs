using Cysharp.Threading.Tasks;
using LightSide;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VS.Runtime.Shared.UI;

namespace VS.Runtime.Core.UI
{
    public class ScorePopupView : Popup
    {
        [SerializeField] private UniText _scoreText;
        [SerializeField] private Button _submitButton;

        private readonly CompositeDisposable _disposables = new();
        private ResultPopupViewModel _vm;

        [Inject]
        public void Construct(ResultPopupViewModel vm)
        {
            _vm = vm;
        }

        protected override UniTask OnOpenStart()
        {
            base.OnOpenStart();
            _vm.Score.Subscribe(OnScoreChanged).AddTo(_disposables);
            _submitButton.onClick.AddListener(OnSubmitClicked);
            
            return UniTask.CompletedTask;
        }

        protected override UniTask OnCloseStart()
        {
            base.OnCloseStart();
            Dispose();
            
            return UniTask.CompletedTask;
        }
        
        private void OnDestroy() => 
            Dispose();

        private void Dispose()
        {
            _disposables.Dispose();
            _vm?.Disposables.Dispose();
        }

        private void OnScoreChanged(int score) => _scoreText.Text = score.ToString();

        private void OnSubmitClicked() => _vm.Submit.Execute(Unit.Default);
    }
}
