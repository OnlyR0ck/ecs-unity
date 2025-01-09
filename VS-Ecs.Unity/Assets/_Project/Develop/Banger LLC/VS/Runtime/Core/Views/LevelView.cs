using UnityEngine;
using VS.Runtime.Core.Interfaces;

namespace VS.Runtime.Core.Views
{
    public class LevelView : View, ILevel
    {
        [field: SerializeField] public Transform GridSpawnPoint { get; private set; }
        [field: SerializeField] public Transform GridSpawnRoot { get; private set; }
    }
}