using Cysharp.Threading.Tasks;
using VContainer;
using VS.Runtime.Core.Infrastructure;

namespace VS.Runtime.Shared.UI
{
    public sealed class PopupService : BaseViewService, IPopupService
    {
        [Inject]
        public PopupService(IObjectResolver resolver, IViewSourceProvider source, IUISceneReferences sceneRefs)
            : base(resolver, source, sceneRefs.PopupsRoot) { }

        public UniTask Show<TView>() where TView : BaseView => ShowBase<TView>();
        public UniTask Hide<TView>() where TView : BaseView => HideBase<TView>();
    }
}