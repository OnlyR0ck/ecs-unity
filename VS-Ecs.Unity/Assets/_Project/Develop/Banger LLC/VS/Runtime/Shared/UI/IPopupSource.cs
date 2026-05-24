namespace VS.Runtime.Shared.UI
{
    public interface IPopupSource
    {
        BaseView GetPrefab<TView>() where TView : BaseView;
    }
}
