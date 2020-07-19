using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Components
{
    [AddComponentMenu("Framework/Components/InputKeyControl")]
    public class InputKeyControl : MonoBehaviour
    {
        public KeyCode Key = KeyCode.Return;
        public Selectable Self;
        public UnityEvent OnKeyDown;

        private void Reset()
        {
            Self = GetComponent<Selectable>();
        }

        public void Update()
        {
            if (Self == null || EventSystem.current.currentSelectedGameObject == gameObject)
            {
                if (Input.GetKeyDown(Key))
                {
                    OnKeyDown.Invoke();
                }
            }
        }
    }
}