using VS.Runtime.Services.Grid;
using VS.Runtime.Services.Input;
using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Infrastructure;
using VS.Runtime.Core.Interfaces;
using VS.Runtime.Core.Views;
using VS.Runtime.Core.Models;

namespace VS.Runtime.Test
{
    public class TestLifetimeScope : LifetimeScope
    {
        [SerializeField] private ResourcesContainer _resourcesContainer;
        [SerializeField] private ShootingConfig _shootingConfig;
        [SerializeField] private GridSettingsConfig _gridSettingsConfig;
        [SerializeField] private InputHandlerService _inputHandler;
        [SerializeField] private CoreGameSceneRefs _refs;
        [SerializeField] private LevelView _levelView;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            //_poolsContainer.Init();
            //_poolsContainer.PrePool();
            
            builder.RegisterInstance(_resourcesContainer);
            builder.RegisterInstance(_shootingConfig);
            builder.RegisterInstance(_gridSettingsConfig);
            builder.RegisterInstance(_inputHandler);
            builder.RegisterInstance(_refs).As<ICoreGameSceneRefs>();
            builder.RegisterInstance(_levelView).As<ILevel>();
            builder.Register<GridModel>(Lifetime.Singleton);
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GridParamsService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(new EcsDefaultWorld());
            builder.RegisterEntryPoint<EcsRoot>();
        }
    }
}
