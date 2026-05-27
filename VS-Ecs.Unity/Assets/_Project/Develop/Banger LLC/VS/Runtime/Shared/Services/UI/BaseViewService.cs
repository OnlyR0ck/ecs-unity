using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using VContainer;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace VS.Runtime.Shared.UI
{
    public abstract class BaseViewService
    {
        private readonly IObjectResolver _resolver;
        private readonly IViewSourceProvider _source;
        private readonly Transform _parent;
        private readonly Dictionary<Type, BaseView> _active = new();

        protected BaseViewService(IObjectResolver resolver, IViewSourceProvider source, Transform parent)
        {
            _resolver = resolver;
            _source = source;
            _parent = parent;
        }

        protected async UniTask ShowBase<TView>() where TView : BaseView
        {
            var type = typeof(TView);
            if (_active.TryGetValue(type, out var existing) && existing.IsOpen)
                return;

            var prefab = _source.GetPrefab<TView>();
            var go = _resolver.Instantiate(prefab.gameObject, _parent);
            var view = go.GetComponent<TView>();
            _active[type] = view;
            await view.Open();
        }

        protected async UniTask HideBase<TView>() where TView : BaseView
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