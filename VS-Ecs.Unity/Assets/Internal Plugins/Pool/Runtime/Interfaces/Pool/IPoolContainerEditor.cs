using System.Collections.Generic;
using VS.Pool.Repositories;

namespace VS.Pool.Interfaces
{
    internal interface IPoolContainerEditor
    {
#if UNITY_EDITOR
        internal void AddRepository(PoolRepository repository);

        internal List<string> GetId();
#endif
    }
}