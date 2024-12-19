using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VS.Pool.Data;
using VS.Pool.Interfaces;
using VS.Pool.Repositories;
using VS.Pool.Utils;

namespace VS.Pool
{
    public class Pool : IPool
    {
        private readonly Transform _parent;
        private readonly PoolRepository _repository;

        private readonly Dictionary<string, Queue<PoolObject>> _elements = new();
        private readonly Dictionary<string, List<PoolObject>> _active = new();
        private readonly Dictionary<int, Coroutine> _awaitingRelease = new();

        private Transform _root;

        private Transform Root
        {
            get
            {
                if (_root != null)
                    return _root;

                GameObject gameObject = new($"{_repository.Id}")
                {
                    transform =
                    {
                        parent = _parent,
                        localPosition = Vector3.zero,
                        rotation = Quaternion.identity,
                        localScale = Vector3.one
                    }
                };

                _root = gameObject.transform;
                return _root;
            }
        }

        public Pool(Transform parent, PoolRepository repository)
        {
            _parent = parent;
            _repository = repository;
        }

        public void PrePool()
        {
            foreach (PoolObjectData data in _repository.Data)
                CreateElements(data.InitialCount, data.Prefab);
        }

        T IPool.Get<T>
        (
            string id,
            Transform parent,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            int minObjectInPool
        )
        {
            PoolObject poolObject = GetPrefab(id);
            if (poolObject == null)
            {
                DebugUtil.LogError($"No exist id in {_repository.Id}Repository");
                return null;
            }

            IPool pool = this;
            return pool.Get<T>(poolObject, parent, position, rotation, scale, minObjectInPool);
        }

        T IPool.Get<T>
        (
            PoolObject prefab,
            Transform parent,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            int minObjectInPool
        )
        {
            string id = prefab.Id;

            if (!_elements.ContainsKey(id))
                CreateElements(minObjectInPool, prefab);
            else
            {
                int delta = minObjectInPool - _elements[id].Count;
                if (delta > 0)
                    CreateElements(delta, prefab);
            }

            if (_elements.ContainsKey(id) && _elements[id].Count != 0)
                return PeekElement<T>(id, parent, position, rotation, scale);

            return AddElement<T>(prefab, parent, position, rotation, scale);
        }

        void IPool.ReleaseAfter(PoolObject poolObject, float waitForSeconds)
        {
            int id = poolObject.GetInstanceID();
            if (_awaitingRelease.ContainsKey(id))
                return;

            _awaitingRelease.Add
            (
                id,
                poolObject.StartCoroutine(ReleaseAfter(poolObject, waitForSeconds))
            );
        }

        private IEnumerator ReleaseAfter(PoolObject poolObject, float waitForSeconds)
        {
            yield return new WaitForSeconds(waitForSeconds);
            IPool pool = this;
            pool.Release(poolObject);
        }

        void IPool.Release(PoolObject poolObject)
        {
            OnDeactivate(poolObject);
        }

        void IPool.ReleaseAllId(string id)
        {
            List<PoolObject> poolObjects = new(_active[id]);

            foreach (PoolObject poolObject in poolObjects)
                OnDeactivate(poolObject);
        }

        public void ReleaseAll()
        {
            List<PoolObject> poolObjects = new();

            foreach (List<PoolObject> objects in _active.Values)
                poolObjects.AddRange(objects);

            foreach (PoolObject obj in poolObjects)
                OnDeactivate(obj);
        }

        private PoolObject CreateObject
        (
            PoolObject prefab,
            Transform parent = null,
            Vector3 position = default,
            Quaternion rotation = default,
            Vector3 scale = default
        )
        {
            PoolObject poolObject = Object.Instantiate(prefab);
            UpdateTransform(poolObject, parent, position, rotation, scale);

            IPoolable poolable = poolObject;
            (poolable as ICreatable)?.OnCreate();

            return poolObject;
        }

        private void CreateElements(int count, PoolObject prefab)
        {
            for (int i = 0; i < count; i++)
            {
                PoolObject poolObject = CreateObject(prefab, Root, Root.position, Quaternion.identity,
                    Vector3.one);
                poolObject.gameObject.SetActive(false);

                string id = poolObject.Id;
                if (!_elements.ContainsKey(id))
                    _elements[id] = new Queue<PoolObject>();

                _elements[id].Enqueue(poolObject);
            }
        }

        private T AddElement<T>
        (
            PoolObject prefab,
            Transform parent = null,
            Vector3 position = default,
            Quaternion rotation = default,
            Vector3 scale = default
        ) where T : PoolObject
        {
            PoolObject poolObject = CreateObject(prefab, parent, position, rotation, scale);
            return OnActivate<T>(poolObject, poolObject.Id);
        }

        private T PeekElement<T>
        (
            string id,
            Transform parent = null,
            Vector3 position = default,
            Quaternion rotation = default,
            Vector3 scale = default
        ) where T : PoolObject
        {
            PoolObject poolObject = _elements[id].Dequeue();
            UpdateTransform(poolObject, parent, position, rotation, scale);

            poolObject.gameObject.SetActive(true);

            return OnActivate<T>(poolObject, id);
        }

        private T OnActivate<T>(PoolObject poolObject, string id) where T : PoolObject
        {
            IPoolable poolable = poolObject;
            (poolable as IReceivable)?.OnReceived();

            if (!_active.ContainsKey(id))
                _active[id] = new List<PoolObject>();
            
            _active[id].Add(poolObject);
            return poolObject as T;
        }

        private PoolObject GetPrefab(string id)
        {
            foreach (PoolObjectData data in _repository.Data)
            {
                PoolObject prefab = data.Prefab;
                if (id.Equals(prefab.Id))
                    return prefab;
            }

            return null;
        }

        private void OnDeactivate(PoolObject poolObject)
        {
            int instanceId = poolObject.GetInstanceID();
            if (_awaitingRelease.ContainsKey(instanceId))
            {
                poolObject.StopCoroutine(_awaitingRelease[instanceId]);
                _awaitingRelease.Remove(instanceId);
            }
            
            string id = poolObject.Id;
            if (!_active.ContainsKey(id) || _active[id].Count == 0)
                return;

            _active[id].Remove(poolObject);
            if (_active[id].Count == 0)
                _active.Remove(id);

            if (!_elements.ContainsKey(id))
                _elements[id] = new Queue<PoolObject>();

            _elements[id].Enqueue(poolObject);

            poolObject.gameObject.SetActive(false);
            UpdateTransform(poolObject, Root, Root.position, Quaternion.identity, Vector3.one);

            IPoolable poolable = poolObject;
            (poolable as IReleasable)?.OnRelease();
        }

        private static void UpdateTransform
        (
            Component poolObject,
            Transform parent = null,
            Vector3 position = default,
            Quaternion rotation = default,
            Vector3 scale = default
        )
        {
            Transform poolObjectTransform = poolObject.transform;
            poolObjectTransform.parent = parent;
            poolObjectTransform.position = position;
            poolObjectTransform.rotation = rotation;
            poolObjectTransform.localScale = scale == default ? Vector3.one : scale;
        }
    }
}