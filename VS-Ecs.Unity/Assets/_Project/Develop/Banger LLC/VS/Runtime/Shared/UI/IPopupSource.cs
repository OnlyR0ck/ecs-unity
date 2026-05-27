namespace VS.Runtime.Shared.UI
{
    public interface IViewSourceProvider
    {
        BaseView GetPrefab<TView>() where TView : BaseView;
    }
}
