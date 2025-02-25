using System;
using DCFApixels.DragonECS;
using VS.Runtime.Core.Views;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace VS.Runtime.Core.Components
{
    #if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    #endif

    [Serializable]
    public struct CellViewComponent : IEcsTagComponent
    {
        public CellView View;
    }

    #if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    #endif
    public class CellViewComponentTemplate : TagComponentTemplate<CellViewComponent> { }
}