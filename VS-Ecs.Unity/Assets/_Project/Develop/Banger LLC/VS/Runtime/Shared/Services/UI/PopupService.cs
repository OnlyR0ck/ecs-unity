using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace VS.Runtime.Shared.UI
{
    public class PopupService : IPopupService
    {
        private readonly IObjectResolver _resolver;
        private readonly IPopupSource _source;
        private readonly Dictionary<Type, BaseView> _active = new();

        public PopupService(IObjectResolver resolver, IPopupSource source)
        {
            _resolver = resolver;
            _source = source;
        }

        public async UniTask Show<TView>() where TView : BaseView
        {
            var type = typeof(TView);
            if (_active.TryGetValue(type, out var existing) && existing.IsOpen)
                return;

            var prefab = _source.GetPrefab<TView>();
            var go = _resolver.Instantiate(prefab.gameObject);
            var view = go.GetComponent<TView>();
            _active[type] = view;
            await view.Open();
        }

        public async UniTask Hide<TView>() where TView : BaseView
        {
            var type = typeof(TView);
            if (!_active.TryGetValue(type, out var view) || !view.IsOpen)
                return;

            await view.Close();
            Object.Destroy(view.gameObject);
            _active.Remove(type);
        }

        public async UniTask HideAll()
        {
            var views = new List<BaseView>(_active.Values);
            _active.Clear();
            var tasks = new List<UniTask>(views.Count);
            foreach (var view in views)
            {
                if (view.IsOpen)
                    tasks.Add(CloseAndDestroy(view));
            }
            if (tasks.Count > 0)
                await UniTask.WhenAll(tasks);
        }

        private async UniTask CloseAndDestroy(BaseView view)
        {
            await view.Close();
            Object.Destroy(view.gameObject);
        }
    }
}
