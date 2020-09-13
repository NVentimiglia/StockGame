using DG.Tweening;
using Framework;
using Framework.Components;
using Stocks.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Stocks.Views
{
    /// <summary>
    /// Horizontal bar chart of volume candles
    /// </summary>
    [ExecutionOrder(ExecutionOrderAttribute.DEFAULT_4)]
    public class StockVolumeChart : MonoBehaviour
    {
        [Range(0, 1000)]
        public int Low = 0;

        [Range(10, 1000)]
        public int High = 100;

        public float XSpace = 10;

        public int MaxPeriod = 70;

        public float TweenTime = .33f;

        public ViewFactory Factory;

        private List<StockVolumeChartBar> _bars = new List<StockVolumeChartBar>();
        private Tweener _tween;

        private void Awake()
        {
            //anchor to bottom
            (Factory.Prefab.transform as RectTransform).anchorMin = new Vector2(0, 0);
            (Factory.Prefab.transform as RectTransform).anchorMax = new Vector2(0, 0);
        }

        public void UpdateHigh(int high)
        {
            if(high != High)
            {
                High = high;
                foreach (var bar in _bars)
                {
                    Bind(bar, bar.Model);
                }
            }
        }

        public void UpdateLow(int low)
        {
            if(low != Low)
            {
                Low = low;
                foreach (var bar in _bars)
                {
                    Bind(bar, bar.Model);
                }
            }
        }

        public void OnStream(IStream<StockBarData> stream)
        {
            foreach (var item in stream.Get())
            {
                UpdateData(item);
            }

            stream.Subscribe(UpdateData);
        }

        public void UpdateData(StockBarData model)
        {
            if (_bars.Count > 0)
            {
                var last = _bars[_bars.Count - 1];
                if (last.Model.Equals(model))
                {
                    Bind(last, model);
                    return;
                }
            }

            var view = Factory.Add().GetComponent<StockVolumeChartBar>();
            _bars.Add(view);
            Bind(view, model);
            
            if (_bars.Count > MaxPeriod)
            {
                if (_tween != null)
                {
                    _tween.Complete();
                }

                var first = _bars[0];
                _bars.RemoveAt(0);
                _tween = transform.DOMoveX(Factory.Root.position.x - XSpace, TweenTime)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Factory.Remove(first.gameObject);
                    _tween = null;
                });
            }
        }

        private void Bind(StockVolumeChartBar view, StockBarData model)
        {
            view.Model = model;

            var x = XSpace * model.Period;
            (view.transform as RectTransform).anchoredPosition = new Vector3(x,0);

            //size
            var height = (Factory.Root as RectTransform).rect.height;
            var percent = (float)model.Volume / (float)High;
            var result = Mathf.Max(1, (percent * height));
            view.Body.sizeDelta = new Vector2(view.Body.sizeDelta.x, result);

            // paint
            if (model.Open > model.Close)
            {
                view.PaintRed();

            }
            else
            {
                view.PaintGreen();
            }

        }
    }
}
