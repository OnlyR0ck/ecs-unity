using Cysharp.Threading.Tasks;

namespace VS.Runtime.Shared.UI
{
    public interface IPopupService
    {
        UniTask Show<TView>() where TView : BaseView;
        UniTask Hide<TView>() where TView : BaseView;
        UniTask HideAll();
    }
}
