using Cysharp.Threading.Tasks;

namespace VS.Runtime.Shared.UI
{
    public interface IScreenService
    {
        UniTask Show<TView>() where TView : Screen;
        UniTask Hide<TView>() where TView : Screen;
    }
}