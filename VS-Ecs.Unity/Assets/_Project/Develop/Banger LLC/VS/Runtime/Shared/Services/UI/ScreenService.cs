using Cysharp.Threading.Tasks;
using VContainer;
using VS.Runtime.Core.Infrastructure;

namespace VS.Runtime.Shared.UI
{
    public sealed class ScreenService : BaseViewService, IScreenService
    {
        [Inject]
        public ScreenService(IObjectResolver resolver, IViewSourceProvider source, IUISceneReferences refs)
            : base(resolver, source, refs.ScreensRoot) { }

        public async UniTask Show<TView>() where TView : Screen
        {
            await HideAll();
            await ShowBase<TView>();
        }

        public UniTask Hide<TView>() where TView : Screen => HideBase<TView>();
    }
}