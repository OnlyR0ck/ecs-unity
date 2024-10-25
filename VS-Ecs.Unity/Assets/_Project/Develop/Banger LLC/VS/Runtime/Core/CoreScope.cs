using VContainer;
using VContainer.Unity;

namespace VS.Runtime.Core
{
    public sealed class CoreScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CoreFlow>();
        }
    }
}