using System;
using TMPro;
using UnityEngine;

namespace Framework.Components
{

    [AddComponentMenu("Framework/Components/TextBinder")]
    public class TextBinder : MonoBehaviour
    {
        public TextMeshProUGUI Target;

        // {0:C} money
        // {0:n} comma number
        public string FormatOption;

        public void UpdateText(int value)
        {
            if (string.IsNullOrEmpty(FormatOption))
            {
                Target.text = value.ToString();
            }
            else
            {
                Target.text = string.Format(FormatOption, value);
            }
        }

        public void UpdateText(float value)
        {
            if (string.IsNullOrEmpty(FormatOption))
            {
                Target.text = value.ToString();
            }
            else
            {
                Target.text = string.Format(FormatOption, value);
            }
        }

        public void UpdateText(DateTime value)
        {
            if (value == null)
            {
                Target.text = string.Empty;
            }
            else if (string.IsNullOrEmpty(FormatOption))
            {
                Target.text = value.ToString();
            }
            else
            {
                Target.text = string.Format(FormatOption, value);
            }
        }

        public void UpdateText(TimeSpan value)
        {
            if (value == null)
            {
                Target.text = string.Empty;
            }
            else if (string.IsNullOrEmpty(FormatOption))
            {
                Target.text = value.ToString();
            }
            else
            {
                Target.text = string.Format(FormatOption, value);
            }
        }

        public void UpdateText(object value)
        {
            if(value == null)
            {
                Target.text = string.Empty;
            }
            else if (string.IsNullOrEmpty(FormatOption))
            {
                Target.text = value.ToString();
            }
            else
            {
                Target.text = string.Format(FormatOption, value);
            }
        }

        private void Reset()
        {
            Target = GetComponent<TextMeshProUGUI>();
        }
    }
}