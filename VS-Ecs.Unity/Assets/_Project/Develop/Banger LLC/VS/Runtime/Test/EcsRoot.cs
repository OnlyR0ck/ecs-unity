using System;
using DCFApixels.DragonECS;
using VContainer;
using VContainer.Unity;
using VS.Runtime.Core.Modules;
using VS.Runtime.Core.Systems;
using VS.Runtime.Extensions;

namespace VS.Runtime.Test
{
    public class EcsRoot : ITickable, IInitializable, IDisposable
    {
        private EcsPipeline _pipeline;
        private EcsDefaultWorld _world;
        private readonly IObjectResolver _objectResolver;

        [Inject]
        public EcsRoot(IObjectResolver objectResolver, EcsDefaultWorld world)
        {
            _objectResolver = objectResolver;
            _world = world;
        }
        public void Initialize()
        {
            EcsPipeline.Builder builder = EcsPipeline.New();
            
            
            //I'm registering systems as transient to prevent access from one system to another
            builder.Inject(_world)
                .Add(_objectResolver.Instantiate<CannonModule>(Lifetime.Transient))
                .Add(_objectResolver.Instantiate<MoveAlongPathSystem>(Lifetime.Transient))
                .Add(_objectResolver.Instantiate<CleanUpSystem>(Lifetime.Transient))
                /*.Add(_objectResolver.Instantiate<CannonAimLineSystem>(Lifetime.Transient))
                .Add(_objectResolver.Instantiate<CannonShootSystem>(Lifetime.Transient))
                .Add(_objectResolver.Instantiate<MoveAlongPathSystem>(Lifetime.Transient))
                .Add(_objectResolver.Instantiate<CleanUpSystem>(Lifetime.Transient))*/;
            
            _pipeline = builder.BuildAndInit();
        }

        public void Tick()
        {
            _pipeline.Run();
        }

        public void Dispose()
        {
            _pipeline.Destroy();
            _pipeline = null;
            _world.Destroy();
            _world = null;
        }
    }
}