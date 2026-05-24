using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

namespace VS.Runtime.Shared.UI
{
    public class Popup : BaseView
    {
        [SerializeField] private float _animDuration = 0.3f;
        [SerializeField] private Transform _root;

        protected override async UniTask PlayOpenAnimation()
        {
            var root = _root != null ? _root : transform;
            root.localScale = Vector3.zero;
            await LMotion.Create(0f, 1f, _animDuration)
                .WithEase(Ease.OutBack)
                .Bind(s => root.localScale = Vector3.one * s)
                .ToUniTask();
        }

        protected override async UniTask PlayCloseAnimation()
        {
            var root = _root != null ? _root : transform;
            root.localScale = Vector3.zero;
            await LMotion.Create(1f, 0f, _animDuration)
                .WithEase(Ease.InBack)
                .Bind(s => root.localScale = Vector3.one * s)
                .ToUniTask();
        }
    }
}
