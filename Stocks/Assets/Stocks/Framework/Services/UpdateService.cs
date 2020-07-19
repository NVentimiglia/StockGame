using UnityEngine;

namespace Framework
{
    [ExecutionOrder(ExecutionOrderAttribute.FRAMEWORK_0)]
    [AddComponentMenu("Framework/UpdateService")]
    public class UpdateService : MonoBehaviour
    {
        private static UpdateService _instance;
        public static UpdateService Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("UpdateService");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<UpdateService>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
        }
    }
}
