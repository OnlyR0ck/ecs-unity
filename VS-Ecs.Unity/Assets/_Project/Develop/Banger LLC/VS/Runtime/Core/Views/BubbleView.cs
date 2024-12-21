using Arch.Unity.Conversion;
using VS.Pool;
using VS.Pool.Interfaces;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Views
{
    public class BubbleView : PoolObject, IReleasable, IComponentConverter
    {
        public void OnRelease() => 
            gameObject.SetActive(false);

        public void Convert(IEntityConverter converter)
        {
            converter.AddComponent(new ViewComponent<BubbleView>(this));
        }
    }
}