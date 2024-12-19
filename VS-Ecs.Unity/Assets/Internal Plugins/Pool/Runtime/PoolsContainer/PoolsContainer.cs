using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VS.Pool.Interfaces;
using VS.Pool.Repositories;
using VS.Pool.Utils;

namespace VS.Pool.Container
{
    public class PoolsContainer : MonoBehaviour, IPoolContainer, IPoolContainerEditor
    {
        [SerializeField] private List<PoolRepository> _poolRepositories;

        private readonly Dictionary<string, Pool> _pools = new();
        
        public void Init()
        {
            foreach (PoolRepository repository in _poolRepositories)
                _pools.Add(repository.Id, new Pool(transform, repository));
        }

        public void PrePool()
        {
            foreach (Pool pool in _pools.Values)
                pool.PrePool();
        }

        IPool IPoolContainer.GetPool(string id)
        {
            if (!_pools.ContainsKey(id))
            {
                DebugUtil.LogError($"It is impossible to get pool. No exist id: {id}");
                return null;
            }

            return _pools[id];
        }

        void IPoolContainer.ReleasePool(string id)
        {
            if (!_pools.ContainsKey(id))
            {
                DebugUtil.LogError($"It is impossible to release pool. No exist id: {id}");
                return;
            }

            _pools[id].ReleaseAll();
        }

        void IPoolContainer.ReleaseAllPools()
        {
            Dictionary<string, Pool>.KeyCollection ids = _pools.Keys;

            foreach (string id in ids)
                _pools[id].ReleaseAll();
        }

#if UNITY_EDITOR
        void IPoolContainerEditor.AddRepository(PoolRepository repository) => _poolRepositories.Add(repository);

        List<string> IPoolContainerEditor.GetId() => _poolRepositories.Select(repository => repository.Id).ToList();
#endif
    }
}