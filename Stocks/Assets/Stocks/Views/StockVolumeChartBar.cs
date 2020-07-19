using Framework;
using Stocks.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Stocks.Views
{
    [ExecutionOrder(ExecutionOrderAttribute.DEFAULT_4)]
    public class StockVolumeChartBar : MonoBehaviour
    {
        public StockBarData Model;
        public RectTransform Body;
        public Image Renderer;

        public Color Red;
        public Color Green;

        private void Awake()
        {
            //anchor to bottom
            Body.pivot = new Vector2(.5f, 0);
            Body.anchorMin = new Vector2(.5f, 0);
            Body.anchorMax = new Vector2(.5f, 0);
        }

        public void PaintGreen()
        {
            Renderer.color = Green;
        }

        public void PaintRed()
        {
            Renderer.color = Red;
        }
    }
}
