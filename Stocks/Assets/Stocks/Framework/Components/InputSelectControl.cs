using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Components
{
    [AddComponentMenu("Framework/Components/InputSelectControl")]
    public class InputSelectControl : MonoBehaviour
    {
        public KeyCode Key = KeyCode.Tab;
        public Selectable Self;
        public Selectable Next;
        public bool AutoSelect;

        private void Reset()
        {
            Self = GetComponent<Selectable>();
        }

        private void OnEnable()
        {
            if (AutoSelect)
            {
                Self.OnPointerDown(new PointerEventData(EventSystem.current));
            }
        }

        public void Update()
        {
            if (!Next)
                return;

            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                if (Input.GetKeyDown(Key))
                {
                    Next.OnPointerDown(new PointerEventData(EventSystem.current));
                }
            }
        }
    }
}