using Arch.Unity;
using Arch.Unity.Conversion;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace VS.Runtime.Test
{
    public class TestLifetimeScope : LifetimeScope
    {
        [SerializeField] private ResourcesContainer _resourcesContainer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            builder.RegisterInstance(_resourcesContainer);
            builder.UseNewArchApp(Lifetime.Scoped, EntityConversion.DefaultWorld, systems =>
            {
                systems.Add<CubeSpawnSystem>();
                systems.Add<RotationSystem>();
            });
        }
    }
}