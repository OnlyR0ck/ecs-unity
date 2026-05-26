using UnityEngine;
using VContainer;
using VContainer.Unity;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Infrastructure;
using VS.Runtime.Meta.Infrastructure;
using VS.Runtime.Meta.UI;
using VS.Runtime.Shared.UI;

namespace VS.Runtime.Meta
{
    public sealed class MetaScope : LifetimeScope
    {
        [SerializeField] private ViewSourceConfig _viewSourceConfig;
        [SerializeField] private MetaGameSceneReferences _metaGameSceneReferences;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_viewSourceConfig).As<IViewSourceProvider>();
            builder.RegisterInstance(_metaGameSceneReferences).As<IMetaGameSceneReferences, IUISceneReferences>();
            builder.Register<ScreenService>(Lifetime.Singleton).As<IScreenService>();
            builder.Register<LobbyViewModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MetaFlow>();
        }
    }
}