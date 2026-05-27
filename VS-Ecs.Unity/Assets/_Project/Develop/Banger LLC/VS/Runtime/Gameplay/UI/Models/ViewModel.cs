using R3;

namespace VS.Runtime.Core.UI
{
    public class ViewModel
    {
        public readonly CompositeDisposable Disposables = new CompositeDisposable();

        ~ViewModel()
        {
            Disposables.Dispose();
        }
    }
}