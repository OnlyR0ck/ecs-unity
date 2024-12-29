using System;
using DCFApixels.DragonECS;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct BubbleComponent : IEcsComponent
    {
        public BubbleView View;
    }
    
    public class BubbleComponentTemplate : ComponentTemplate<BubbleComponent> {}
}