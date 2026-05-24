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

        [Inject]
        public void Construct(ResultPopupViewModel vm)
        {
            vm.Score.Subscribe(s => _scoreText.Text = s.ToString()).AddTo(_disposables);
            _submitButton.onClick.AddListener(() => vm.Submit.Execute(Unit.Default));
        }

        private void OnDestroy() => _disposables.Dispose();
    }
}
