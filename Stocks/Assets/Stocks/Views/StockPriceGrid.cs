using Framework;
using Framework.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Stocks.Views
{
    [ExecutionOrder(ExecutionOrderAttribute.DEFAULT_4)]
    public class StockPriceGrid : MonoBehaviour
    {
        [Range(0, 1000)]
        public int Low = 0;

        [Range(10, 1000)]
        public int High = 100;

        public int Rows = 11;

        public ViewFactory Factory;

        private float rowHeight;
        private List<StockPriceGridRow> _gridRows = new List<StockPriceGridRow>();

        public void Start()
        {
            var rootRect = Factory.Root as RectTransform;
            rowHeight = rootRect.rect.height / Rows;

            Factory.Clear();

            for (int i = 0; i < Rows; i++)
            {
                var price = GetPriceForRow(i);
                var y = (rowHeight * (i + .5f));
                var view = Factory.Add().GetComponent<StockPriceGridRow>();
                view.Label.text = Mathf.RoundToInt(price).ToString();
                view.transform.localPosition = new Vector3(0, y);
                _gridRows.Add(view);
            }
        }

        [ContextMenu("Reprice")]
        public void Reprice()
        {
            for (int i = 0; i < _gridRows.Count; i++)
            {
                var view = _gridRows[i];
                var price = GetPriceForRow(i);
                view.Label.text = Mathf.RoundToInt(price).ToString();
            }
        }

        int GetPriceForRow(int index)
        {
            return (int)Mathf.LerpUnclamped(Low, High, (float)index / ((float)Rows - 1f));
        }

        public void SetHigh(int high)
        {
            High = high;
            Reprice();
        }

        public void SetLow(int low)
        {
            Low = low;
            Reprice();
        }
    }
}
