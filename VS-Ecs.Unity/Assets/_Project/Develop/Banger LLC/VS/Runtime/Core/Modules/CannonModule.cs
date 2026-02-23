using DCFApixels.DragonECS;
using Unity.IL2CPP.CompilerServices;
using VContainer;
using VS.Runtime.Core.Systems;
using VS.Runtime.Extensions;
using VS.Runtime.Test;

#if ENABLE_INPUT_SYSTEM
using Unity.IL2CPP.CompilerServices;
#endif

namespace VS.Runtime.Core.Modules
{
    #if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    #endif
    class CannonModule : IEcsModule
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public CannonModule(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        public void Import(EcsPipeline.Builder builder)
        {
            builder.Add(_resolver.Instantiate<CannonSpawnSystem>(Lifetime.Transient));
            builder.Add(_resolver.Instantiate<CannonRotationSystem>(Lifetime.Transient));
            builder.Add(_resolver.Instantiate<CannonAimLineSystem>(Lifetime.Transient));
            builder.Add(_resolver.Instantiate<CannonShootSystem>(Lifetime.Transient));
        }
    }
}