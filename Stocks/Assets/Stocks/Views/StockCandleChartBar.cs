using Framework;
using Stocks.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Stocks.Views
{
    /// <summary>
    /// Candle chart item
    /// </summary>
    [ExecutionOrder(ExecutionOrderAttribute.DEFAULT_4)]
    public class StockCandleChartBar : MonoBehaviour
    {
        public StockBarData Model;
        public RectTransform Wick;
        public RectTransform Body;
        public Image[] Renderers;

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
            for (int i = 0; i < Renderers.Length; i++)
            {
                Renderers[i].color = Green;
            }
        }

        public void PaintRed()
        {
            for (int i = 0; i < Renderers.Length; i++)
            {
                Renderers[i].color = Red;
            }
        }
    }
}
