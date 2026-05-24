using UnityEngine;

namespace VS.Runtime.Core.Views
{
    public class GridView : EcsView
    {
        [field: SerializeField] public Transform GridSpawnRoot { get; private set; }

    }
}