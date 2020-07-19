using System;
using UnityEngine;

namespace Framework.Components
{
    [Serializable]
    [AddComponentMenu("Framework/Components/ViewBase")]
    public class ViewBase : MonoBehaviour
    {
        [SerializeField]
        protected ObservableBool Visible;

        private bool _isVirgin = true;
        public bool IsVisible { get { return Visible.Value; } set { Visible.Set(value); } }

        public void Open()
        {
            SetOpen(true);
        }

        public void Close()
        {
            SetOpen(false);
        }

        public void SetOpen(bool isOpen)
        {
            if (!_isVirgin)
            {
                if (isOpen == Visible.Value)
                    return;
            }
            _isVirgin = false;

            Visible.Set(isOpen);
            OnOpenClose(isOpen);
        }


        public void ToggleOpen()
        {
            SetOpen(!IsVisible);
        }

        void OnOpenClose(bool isOpen)
        {
            if (isOpen)
                OnOpen();
            else
                OnClose();
        }

        protected virtual void OnOpen()
        {

        }
        protected virtual void OnClose()
        {

        }
    }
}