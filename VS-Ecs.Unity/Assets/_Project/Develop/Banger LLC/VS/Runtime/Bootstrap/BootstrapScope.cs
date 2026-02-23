using UnityEngine;
using UnityEngine.EventSystems;
using VS.Runtime.Services;
using VContainer;
using VContainer.Unity;

namespace VS.Runtime.Bootstrap
{
    public sealed class BootstrapScope : LifetimeScope
    {
        [SerializeField] private EventSystem _eventSystem;
        
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_eventSystem);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LoadingService>(Lifetime.Singleton);
            builder.Register<SceneService>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}