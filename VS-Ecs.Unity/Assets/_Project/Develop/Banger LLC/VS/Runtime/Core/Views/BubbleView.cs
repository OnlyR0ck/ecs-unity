using VS.Pool;
using VS.Pool.Interfaces;

namespace VS.Runtime.Core.Views
{
    public class BubbleView : PoolObject, IReleasable
    {
        public void OnRelease() => 
            gameObject.SetActive(false);
    }
}