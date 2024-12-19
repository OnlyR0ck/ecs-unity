using UnityEngine;
using VS.Pool.Interfaces;

namespace VS.Pool
{
    public class PoolObject : MonoBehaviour, IPoolable
    {
        [SerializeField] private string _id;

        public string Id => string.IsNullOrEmpty(_id) ? gameObject.name.Replace(Constants.CloneName, "") : _id;
    }
}