using UnityEngine;
using VS.Pool.Data;

namespace VS.Pool.Repositories
{
    public class PoolRepository : ScriptableObject
    {
        public string Id;

        [field: SerializeField] public PoolObjectData[] Data { get; private set; }
    }
}