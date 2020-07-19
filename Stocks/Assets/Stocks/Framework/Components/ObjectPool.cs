//#define VERBOSE
using System;
using System.Collections.Generic;

namespace Framework.Components
{
    [Serializable]
    public class ObjectPool<T> : IDisposable
    {
        Queue<T> _pool = new Queue<T>();
        Func<T> _factory;

        public ObjectPool(Func<T> factory)
        {
            _factory = factory;
            _pool = new Queue<T>();
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
        public T Rent()
        {
            T obj;

            if (_pool == null || _pool.Count == 0)
            {
#if VERBOSE
                UnityEngine.Debug.Log("Pool.Instantiate " + _prefab);
#endif
                return Instantiate();
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
        public void Return(T obj)
        {
            if (obj == null)
                return;

#if VERBOSE
            UnityEngine.Debug.Log("Pool.Enqueue " + _prefab);
#endif
            _pool.Enqueue(obj);
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
                _pool.Dequeue();
            }
        }

        /// <summary>
        /// Destroys all objects
        /// </summary>
        public void Dispose()
        {
            Clean();
            _factory = null;
        }

        /// <summary>
        /// New instance, not pooled
        /// </summary>
        /// <returns></returns>
        public T Instantiate()
        {
            return _factory();
        }
    }
}
