using System.Collections.Generic;
using UnityEngine;
using VS.Pool.Interfaces;
using VS.Pool.Repositories;

namespace VS.Pool.Container
{
    public class PoolsContainersEditor : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private PoolsContainer _poolsContainer;

        internal void AddPoolRepository(PoolRepository repository) =>
            (_poolsContainer as IPoolContainerEditor)?.AddRepository(repository);

        internal List<string> GetId() => (_poolsContainer as IPoolContainerEditor)?.GetId();
#endif
    }
}