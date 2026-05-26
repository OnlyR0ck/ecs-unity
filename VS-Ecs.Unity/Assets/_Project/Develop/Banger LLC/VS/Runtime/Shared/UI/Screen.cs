using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

namespace VS.Runtime.Shared.UI
{
    public class Screen : BaseView
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _animDuration = 0.3f;

        protected override async UniTask PlayOpenAnimation()
        {
            _canvasGroup.alpha = 0f;
            await LMotion.Create(0f, 1f, _animDuration)
                .Bind(_canvasGroup, static (v, cg) => cg.alpha = v)
                .ToUniTask();
        }

        protected override async UniTask PlayCloseAnimation()
        {
            await LMotion.Create(1f, 0f, _animDuration)
                .Bind(_canvasGroup, static (v, cg) => cg.alpha = v)
                .ToUniTask();
        }
    }
}
