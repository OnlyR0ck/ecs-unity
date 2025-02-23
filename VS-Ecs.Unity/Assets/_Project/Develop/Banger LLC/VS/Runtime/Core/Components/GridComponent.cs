using DCFApixels.DragonECS;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Core.Components
{
    public struct GridComponent : IEcsComponent
    {
        public GridView View;
        public CellView [,] Cells;
    }
    
    public class GridComponentTemplate : ComponentTemplate<GridComponent>{}
}