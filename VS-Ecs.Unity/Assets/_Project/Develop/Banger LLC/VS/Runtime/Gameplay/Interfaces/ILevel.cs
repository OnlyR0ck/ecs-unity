using UnityEngine;

namespace VS.Runtime.Core.Interfaces
{
    public interface ILevel
    {
        Transform GridSpawnPoint { get; }
        Transform GridSpawnRoot { get; }
        Transform CannonSpawnRoot { get; }
    }
}