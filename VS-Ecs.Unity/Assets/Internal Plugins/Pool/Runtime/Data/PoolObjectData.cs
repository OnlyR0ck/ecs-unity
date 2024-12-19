using System;
using UnityEngine;

namespace VS.Pool.Data
{
    [Serializable]
    public class PoolObjectData
    {
        [field: SerializeField] public PoolObject Prefab { get; private set; }
        [field: SerializeField] public int InitialCount { get; private set; }
    }
}