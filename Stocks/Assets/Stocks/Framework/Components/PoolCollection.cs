//#define VERBOSE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Object pool for game objects with a controlling component
    /// </summary>
    [Serializable]
    public class PoolCollection : IDisposable
    {
        public Transform Root { get; private set; }
        public GameObject Prefab { get; private set; }
        public int Id { get; set; }

        Queue<GameObject> _pool = new Queue<GameObject>(32);
        int _maxSize;

        /// <summary>
        /// init the pool
        /// </summary>
        /// <param name="root">root transform</param>
        /// <param name="prefab">prefab to pool</param>
        /// <param name="maxSize">max unused count to keep in memory</param>
        public PoolCollection(Transform root, GameObject prefab, int maxSize = 64)
        {
            Root = root;
            Prefab = prefab;
            _maxSize = maxSize;
            Id = prefab.GetInstanceID();
        }

        /// <summary>
        /// Pool Warming
        /// </summary>
        /// <param name="total">count</param>
        /// <param name="maxPerFrame">uses corroutine</param>
        /// <param name="oncomplete">complete</param>
        public void Init(int total)
        {
            for (int i = 0; i < total; i++)
            {
                Return(Rent());
            }
        }

        /// <summary>
        /// Pool Warming
        /// </summary>
        /// <param name="total">count</param>
        /// <param name="maxPerFrame">uses corroutine</param>
        /// <param name="oncomplete">complete</param>
        public IEnumerator InitAsync(int total, int maxPerFrame)
        {
            int frameCount = 0;
            for (int i = 0; i < total; i++)
            {
                var fab = Instantiate();
                Return(fab);
                frameCount++;
                if (frameCount >= maxPerFrame)
                {
                    frameCount = 0;
                    yield return 1;
                }
            }
        }

        /// <summary>
        /// Count in pool
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _pool.Count;
        }

        /// <summary>
        /// may be receycled
        /// </summary>
        /// <returns></returns>
        public GameObject Rent(Transform root = null)
        {
            GameObject obj;

            if (_pool == null || _pool.Count == 0)
            {
#if VERBOSE
                UnityEngine.Debug.Log("Pool.Instantiate " + _prefab);
#endif
                return Instantiate(root);
            }
            else
            {
#if VERBOSE
                UnityEngine.Debug.Log("Pool.Dequeue " + _prefab);
#endif
                obj = _pool.Dequeue();
            }

            return obj;
        }

        /// <summary>
        /// add to pool
        /// </summary>
        /// <param name="obj"></param>
        public void Return(GameObject obj)
        {
            if (obj == null)
                return;
            
            if (_pool == null || (_maxSize > 0 && _pool.Count >= _maxSize))
            {
#if VERBOSE
                UnityEngine.Debug.Log("Pool.Destroy " + _prefab);
#endif
                Destroy(obj);
            }
            else
            {
                // catch double pools from auto return on disable
                if(!obj.activeSelf && _pool.Contains(obj))
                {
                    return;
                }

#if VERBOSE
                UnityEngine.Debug.Log("Pool.Enqueue " + _prefab);
#endif
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        /// <summary>
        /// Destroy unrented objects from pool
        /// </summary>
        public void Clean()
        {
            if (_pool == null)
                return;

            while (_pool.Count > 0)
            {
                Destroy(_pool.Dequeue());
            }
        }

        /// <summary>
        /// Destroys all objects
        /// </summary>
        public void Dispose()
        {
            _maxSize = 0;
            Clean();
            _pool = null;
            Prefab = null;
            Root = null;
        }

        /// <summary>
        /// New instance, not pooled
        /// </summary>
        /// <returns></returns>
        public GameObject Instantiate(Transform root = null)
        {
            root = root == null ? Root : root;
            var obj = (GameObject)GameObject.Instantiate(Prefab.gameObject, root, false);
            return obj;
        }

        /// <summary>
        /// destroy instance, not pooled
        /// </summary>
        /// <returns></returns>
        public void Destroy(GameObject obj)
        {
            if (obj == null)
                return;
            
            GameObject.Destroy(obj);
        }
    }
}
