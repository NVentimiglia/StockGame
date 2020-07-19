using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

namespace Framework.Components
{
    /// <summary>
    /// Shared pool singleton
    /// </summary>
    [ExecutionOrder(ExecutionOrderAttribute.FRAMEWORK_0)]
    [AddComponentMenu("Framework/Components/PoolManager")]
    public class PoolManager : MonoBehaviour
    {
        static Dictionary<int, PoolCollection> Pools = new Dictionary<int, PoolCollection>();
        static Dictionary<GameObject, int> PrefabIds = new Dictionary<GameObject, int>();
        static Dictionary<int, int> InstanceToPool = new Dictionary<int, int>();
        static PoolManager Instance;
        public static Transform Root { get { return Instance.transform; } }
        public static bool IsAwake { get { return Instance != null; } }

        [Serializable]
        public class PoolConfig
        {
            public GameObject View;
            public int MaxCount = 16;
            public int InitCount = 0;
        }

        [HideInInspector]
        public List<PoolConfig> PoolConfigs;

        public int MaxPerFrame = 1;

        void Awake()
        {
            Instance = this;

            for (int i = 0; i < PoolConfigs.Count; i++)
            {
                var config = PoolConfigs[i];
                PrefabIds.Add(config.View, i);
                Pools.Add(i, new PoolCollection(transform, config.View, config.MaxCount));
            }

            StartCoroutine(InitAsync());
        }

        void OnDestroy()
        {
            Instance = null;
        }

        [ContextMenu("InitResources")]
        public void InitResources()
        {
            var prefabs = new List<UnityEngine.Object>();
            prefabs.AddRange(Resources.LoadAll("Entities"));
            prefabs.AddRange(Resources.LoadAll("FX"));
            prefabs.AddRange(Resources.LoadAll("Weapons"));

            PoolConfigs.Clear();

            PoolConfigs = prefabs.Select(o =>
            {
                return new PoolConfig
                {
                    View = o as GameObject,
                    InitCount = 0,
                    MaxCount = 64,

                };
            }).ToList();
        }

        IEnumerator InitAsync()
        {
            //Pre Warm
            for (int i = 0; i < PoolConfigs.Count; i++)
            {
                var config = PoolConfigs[i];
                var pool = GetPool(config.View);
                if (config.InitCount > 0)
                {
                    yield return StartCoroutine(pool.InitAsync(config.InitCount, MaxPerFrame));
                }
            }
        }

        PoolCollection GetPool(int id)
        {
            PoolCollection pool;
            if (!Pools.TryGetValue(id, out pool))
            {
                UnityEngine.Debug.LogError("Pool Not Found");
                return null;
            }
            return pool;
        }

        PoolCollection GetPool(GameObject prefab)
        {
            var id = GetPoolFromObjectId(prefab);
            return GetPool(id);
        }

        int AddPoolFromObjectId(GameObject go)
        {
            int id;
            if (InstanceToPool.TryGetValue(go.GetInstanceID(), out id))
            {
                return id;
            }
            if (PrefabIds.TryGetValue(go, out id))
            {
                return id;
            }

            PrefabIds.Add(go, Pools.Count);
            Pools.Add(Pools.Count, new PoolCollection(transform, go));
            return Pools.Count - 1;
        }

        //

        public static int GetPoolFromObjectId(GameObject go)
        {
            int id;
            if (InstanceToPool.TryGetValue(go.GetInstanceID(), out id))
            {
                return id;
            }
            if (PrefabIds.TryGetValue(go, out id))
            {
                return id;
            }
            return Instance.AddPoolFromObjectId(go);
        }

        public static GameObject Rent(GameObject prefab, Transform root = null)
        {
            var id = GetPoolFromObjectId(prefab);
            return Rent(id, root);
        }

        public static GameObject RentEffect(GameObject prefab, Transform root, bool parent = false)
        {
            var id = GetPoolFromObjectId(prefab);
            var fx = Rent(id);
            fx.transform.position = root.position;
            fx.transform.rotation = root.rotation;
            fx.SetActive(true);
            if (parent)
            {
                fx.transform.parent = root;
            }
            return fx;
        }

        public static GameObject RentEffect(GameObject prefab, Vector3 position)
        {
            return RentEffect(prefab, position, Quaternion.identity);
        }

        public static GameObject RentEffect(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var id = GetPoolFromObjectId(prefab);
            var fx = Rent(id);
            fx.transform.position = position;
            fx.transform.rotation = rotation;
            fx.SetActive(true);
            return fx;
        }

        public static GameObject Rent(int prefabId, Transform root = null)
        {
            if (Instance == null)
            {
                UnityEngine.Debug.LogError("PoolManager not awake");
                return null;
            }

            var pool = Instance.GetPool(prefabId);
            var instance = pool.Rent(root);
            var id = instance.GetInstanceID();
            InstanceToPool.Add(id, prefabId);
            return instance;
        }

        public static void Return(GameObject instance)
        {
            if (instance == null)
            {
                return;
            }

            var poolId = GetPoolFromObjectId(instance);
            Return(instance, poolId);
        }

        public static void Return(GameObject instance, int prefabid)
        {
            var id = instance.GetInstanceID();
            InstanceToPool.Remove(id);

            if (instance == null)
            {
                return;
            }

            if (Instance == null)
            {
                GameObject.Destroy(instance);
                return;
            }

            if (prefabid == -1)
            {
                GameObject.Destroy(instance);
            }
            else
            {
                var pool = Instance.GetPool(prefabid);

                if (pool == null)
                {
                    GameObject.Destroy(instance);
                }
                else
                {
                    pool.Return(instance);
                }
            }
        }
    }
}
