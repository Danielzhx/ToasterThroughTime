using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT.Utillities.Pooling
{
    public interface IPoolable
    {
        IObjectPool Pool { get; set; }
        void Spawn();
        void Despawn();
    }

    public interface IObjectPool
    {
        void Recycle(object spawn, bool onDisableCalled);
    }

    public interface IObjectPool<in T> : IObjectPool where T : IPoolable
    {
        void Recycle(T spawn, bool onDisableCalled);
    }

    public class ObjectPool<T> : IObjectPool<T> where T : Spawnable, IPoolable
    {
        protected readonly List<T> _pooledObjects;
        protected readonly List<T> _spawnedObjects;

        public int pooledCount => _pooledObjects.Count;
        public int spawnCount => _spawnedObjects.Count;

        protected uint _initialPoolSize;
        protected uint _maxPoolSize;
        protected bool _resizeOnFull = true;

        public T prefab { get; private set; }
        protected GameObject _container;
        public Transform Container => _container.transform;

        public ObjectPool<T> CreateObjectPool(T prefab, Transform poolsContainer = null, uint initialPoolSize = 10, bool resizeOnFull = true, uint maxPoolSize = 0, int instancesPerFrame = 1)
        {
            this.prefab = prefab;
            _initialPoolSize = initialPoolSize;

            if (initialPoolSize > maxPoolSize)
                maxPoolSize = initialPoolSize;

            if (maxPoolSize > initialPoolSize)
                _resizeOnFull = true;

            if (!_container)
                _container = new(string.Format("{0}Pool", prefab.name));
            
            Container.SetParent(poolsContainer ? poolsContainer : PoolManager.Instance.transform, true);
            
            PoolManager.Instance.Add(prefab.name,CreatePools(prefab,_initialPoolSize,instancesPerFrame), Container);

            return this;
        }

        private ObjectPool<T>CreatePools(T prefab, uint initialPoolSize, int instancesPerFrame)
        {
            if (Container.childCount >= initialPoolSize)
            {
                _pooledObjects.Clear();

                foreach (var spawn in Container.GetComponentsInChildren<T>(true))
                {
                    spawn.Pool = this;
                    spawn.name = prefab.name;
                    spawn.gameObject.SetActive(false);
                    _pooledObjects.Add(spawn);
                }
            }
            else
                PoolManager.Instance.StartCoroutine((IEnumerator)InstantiateLoop(prefab, initialPoolSize,
                    instancesPerFrame));

            return this;
        }

        private IEnumerable<WaitForEndOfFrame> InstantiateLoop(T prefab, uint initialPoolSize, int instancesPerFrame)
        {
            int index = 0;

            while (pooledCount < initialPoolSize && Container.childCount < initialPoolSize)
            {
                T spawn = Object.Instantiate(prefab, Container, true);
                
                spawn.Pool = this;
                spawn.name = prefab.name;
                spawn.gameObject.SetActive(false);
                _pooledObjects.Add(spawn);

                index++;

                if (index % instancesPerFrame == 0)
                    yield return new();
            }
        }

        public void Recycle(T spawn, bool onDisableCalled = false)
        {
            if (!_spawnedObjects.Contains(spawn))
                return;

            _spawnedObjects.Remove(spawn);
            _pooledObjects.Add(spawn);
            
            if(!onDisableCalled)            
                spawn.transform.SetParent(Container);
            
        }

        public void Recycle(object spawn, bool onDisableCalled = false)
        {
            if (spawn is T poolable)
            {
                Recycle(spawn, onDisableCalled);
            }
        }
    }
}