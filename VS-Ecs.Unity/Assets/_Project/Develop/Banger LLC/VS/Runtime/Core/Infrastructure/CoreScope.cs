using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Infrastructure;
using VS.Runtime.Core.Interfaces;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Services.Grid;
using VS.Runtime.Services.Input;
using VS.Runtime.Test;

namespace VS.Runtime.Core
{
    public sealed class CoreScope : LifetimeScope
    {
        [SerializeField] private ResourcesContainer _resourcesContainer;
        [SerializeField] private ShootingConfig _shootingConfig;
        [SerializeField] private GridSettingsConfig _gridSettingsConfig;
        [SerializeField] private GameplayRulesConfig _gameplayRulesConfig;
        [SerializeField] private InputHandlerService _inputHandler;
        [SerializeField] private CoreGameSceneRefs _refs;
        [SerializeField] private LevelView _levelView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_resourcesContainer);
            builder.RegisterInstance(_shootingConfig);
            builder.RegisterInstance(_gridSettingsConfig);
            builder.RegisterInstance(_gameplayRulesConfig);
            builder.RegisterInstance(_inputHandler);
            builder.RegisterInstance(_refs).As<ICoreGameSceneRefs>();
            builder.RegisterInstance(_levelView).As<ILevel>();
            builder.Register<GridModel>(Lifetime.Singleton);
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GridParamsService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(new EcsDefaultWorld());
            //builder.RegisterEntryPoint<EcsRoot>();
            builder.RegisterEntryPoint<CoreFlow>();
        }
    }
}