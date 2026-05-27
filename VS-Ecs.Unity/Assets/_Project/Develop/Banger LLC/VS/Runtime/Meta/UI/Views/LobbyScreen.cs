using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Screen = VS.Runtime.Shared.UI.Screen;

namespace VS.Runtime.Meta.UI
{
    public class LobbyScreen : Screen
    {
        [SerializeField] private Button _enterButton;

        private LobbyViewModel _vm;

        [Inject]
        public void Construct(LobbyViewModel vm) => _vm = vm;

        protected override UniTask OnOpenStart()
        {
            _enterButton.onClick.AddListener(OnEnterClicked);
            return UniTask.CompletedTask;
        }

        protected override UniTask OnCloseStart()
        {
            _enterButton.onClick.RemoveListener(OnEnterClicked);
            return UniTask.CompletedTask;
        }

        private void OnEnterClicked() => _vm.EnterGame.Execute(Unit.Default);

        private void OnDestroy() => _vm?.Disposables.Dispose();
    }
}