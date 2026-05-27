using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VS.Runtime.Shared.UI
{
    public abstract class BaseView : MonoBehaviour
    {
        public bool IsOpen { get; private set; }

        public async UniTask Open()
        {
            gameObject.SetActive(true);
            await OnOpenStart();
            await PlayOpenAnimation();
            IsOpen = true;
            await OnOpenComplete();
        }

        public async UniTask Close()
        {
            await OnCloseStart();
            await PlayCloseAnimation();
            IsOpen = false;
            await OnCloseComplete();
            gameObject.SetActive(false);
        }

        protected virtual UniTask OnOpenStart() => UniTask.CompletedTask;
        protected virtual UniTask PlayOpenAnimation() => UniTask.CompletedTask;
        protected virtual UniTask OnOpenComplete() => UniTask.CompletedTask;
        protected virtual UniTask OnCloseStart() => UniTask.CompletedTask;
        protected virtual UniTask PlayCloseAnimation() => UniTask.CompletedTask;
        protected virtual UniTask OnCloseComplete() => UniTask.CompletedTask;
    }
}
