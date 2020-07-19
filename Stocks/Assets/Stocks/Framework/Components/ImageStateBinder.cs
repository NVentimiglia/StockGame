using UnityEngine;
using UnityEngine.UI;

namespace Framework.Components
{

    [AddComponentMenu("Framework/Components/ImageStateBinder")]
    public class ImageStateBinder : MonoBehaviour
    {
        public int Value;

        public Sprite[] Sprites;

        public Image Target;

        public void UpdateSprite(int value)
        {
            Value = value;
            UpdateSprite();
        }

        void UpdateSprite()
        {
            Target.sprite = Sprites[Value];
        }
    }
}