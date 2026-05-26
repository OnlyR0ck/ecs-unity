using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VS.Runtime.Shared.UI;

namespace VS.Runtime.Core.UI
{
    public class WinPopupView : Popup
    {
        [SerializeField] private Button _submitButton;

        private ResultPopupViewModel _vm;

        [Inject]
        public void Construct(ResultPopupViewModel vm)
        {
            _vm = vm;
            _submitButton.onClick.AddListener(OnSubmitClicked);
        }

        private void OnSubmitClicked() => _vm.Submit.Execute(Unit.Default);

        private void OnDestroy() => _vm?.Disposables.Dispose();
    }
}
