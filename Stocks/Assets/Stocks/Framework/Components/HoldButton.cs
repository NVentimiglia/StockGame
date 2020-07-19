using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Components
{
    [AddComponentMenu("Framework/Components/HoldButton")]
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public Button Target;

        public float SendRate = .1f;

        public UnityEvent OnSend;

        private float _delta;
        private bool _active;

        public void OnPointerDown(PointerEventData eventData)
        {
            _active = true;
            _delta = 0;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _active = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _active = false;
        }

        private void Reset()
        {
            Target = GetComponent<Button>();
        }

        private void Update()
        {
            if (_active)
            {
                _delta += Time.deltaTime;

                if (_delta >= SendRate)
                {
                    OnSend.Invoke();
                    _delta = 0;
                }
            }
        }
    }
}