using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VS.Core.Configs.Features;
using VS.Pool.Container;
using VS.Pool.Interfaces;
using VS.Runtime.Core.Infrastructure;
using VS.Runtime.Core.Systems;
using VS.Runtime.Utilities;

namespace VS.Runtime.Test
{
    public class TestLifetimeScope : LifetimeScope
    {
        [SerializeField] private ResourcesContainer _resourcesContainer;
        [SerializeField] private ShootingConfig _shootingConfig;
        [SerializeField] private InputHandlerService _inputHandler;
        [SerializeField] private CoreGameSceneRefs _refs;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            //_poolsContainer.Init();
            //_poolsContainer.PrePool();
            
            builder.RegisterInstance(_resourcesContainer);
            builder.RegisterInstance(_shootingConfig);
            builder.RegisterInstance(_inputHandler);
            builder.RegisterInstance(_refs);
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(new EcsDefaultWorld());
            builder.RegisterEntryPoint<EcsRoot>();
        }
    }
}
