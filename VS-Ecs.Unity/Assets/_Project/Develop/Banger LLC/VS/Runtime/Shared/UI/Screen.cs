using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VS.Runtime.Shared.UI
{
    public class Screen : BaseView
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _animDuration = 0.3f;

        protected override UniTask PlayOpenAnimation() => UniTask.CompletedTask;

        protected override UniTask PlayCloseAnimation() => UniTask.CompletedTask;
    }
}
