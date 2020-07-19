using UnityEngine;

namespace Framework.Components
{
    /// <summary>
    /// Auto Recycle
    /// </summary>
    [AddComponentMenu("Framework/Components/PoolAutoComponent")]
    public class PoolAutoComponent : MonoBehaviour
    {
        public float AutoPoolSeconds = 2f;
        
        private float _delta;

        public GameObject AtrophyEffect;

        void OnEnable()
        {
            _delta = 0;
        }

        void Update()
        {
            _delta += Time.deltaTime;

            if (_delta >= AutoPoolSeconds)
            {
                if (AtrophyEffect)
                {
                    PoolManager.RentEffect(AtrophyEffect, transform);
                }
                PoolManager.Return(gameObject);
            }
        }
    }
}