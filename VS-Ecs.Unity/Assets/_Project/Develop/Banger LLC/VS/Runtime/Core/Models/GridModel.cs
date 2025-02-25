using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Models
{
    public class GridModel
    {
        public GridComponent Grid { get; private set; }

        public void Initialize(ref GridComponent grid)
        {
            Grid = grid;
        }
    }
}