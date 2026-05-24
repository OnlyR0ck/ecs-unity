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

        [Inject]
        public void Construct(ResultPopupViewModel vm)
        {
            _submitButton.onClick.AddListener(() => vm.Submit.Execute(Unit.Default));
        }
    }
}
