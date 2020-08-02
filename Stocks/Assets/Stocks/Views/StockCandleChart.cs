using DG.Tweening;
using Framework;
using Framework.Components;
using Stocks.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Stocks.Views
{
    [ExecutionOrder(ExecutionOrderAttribute.DEFAULT_4)]
    public class StockCandleChart : MonoBehaviour
    {
        [Range(0, 1000)]
        public int Low = 0;

        [Range(10, 1000)]
        public int High = 100;

        public float XSpace = 10;

        public int MaxPeriod = 70;

        public float TweenTime = .33f;

        public ViewFactory Factory;

        private List<StockCandleChartBar> _bars = new List<StockCandleChartBar>();

        private Tweener _tween;

        private void Awake()
        {
            //anchor to bottom
            (Factory.Prefab.transform as RectTransform).anchorMin = new Vector2(0, 0);
            (Factory.Prefab.transform as RectTransform).anchorMax = new Vector2(0, 0);
        }

        public void UpdateHigh(int high)
        {
            if (high != High)
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
            if (low != Low)
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
            if(model.High > High)
            {
                UpdateHigh(model.High);
            }

            if(model.Low < Low)
            {
                UpdateLow(model.Low);
            }

            if (_bars.Count > 0)
            {
                var last = _bars[_bars.Count - 1];
                if (last.Model.Equals(model))
                {
                    Bind(last, model);
                    return;
                }
            }

            var view = Factory.Add().GetComponent<StockCandleChartBar>();
            _bars.Add(view);
            Bind(view, model);

            if (_bars.Count > MaxPeriod)
            {
                if(_tween != null)
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

        private void Bind(StockCandleChartBar view, StockBarData model)
        {
            view.Model = model;

            var x = XSpace * model.Period;
            var y = PriceToY(model.Low);
            (view.transform as RectTransform).anchorMin = new Vector2(0, 0);
            (view.transform as RectTransform).anchorMax = new Vector2(0, 0);
            (view.transform as RectTransform).anchoredPosition = new Vector3(x, y);

            //wick size
            var wickRange = Mathf.Abs(model.High - model.Low);
            var wickSize = Mathf.Max(PriceToY(wickRange), 1);
            view.Wick.sizeDelta = new Vector2(view.Wick.sizeDelta.x, wickSize);


            //body size
            var bodyRange = Mathf.Max(Mathf.Abs(model.Close - model.Open), 1);
            var bodySize = PriceToY(bodyRange);
            view.Body.sizeDelta = new Vector2(view.Body.sizeDelta.x, bodySize);

            //body spread
            var bodyLow = model.Close > model.Open ? model.Open : model.Close;
            var bodySpread = Mathf.Abs(bodyLow - model.Low);
            var bodyY = PriceToY(bodySpread);
            view.Body.anchoredPosition = new Vector2(0, bodyY);

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

        private float PriceToY(float price)
        {
            var percent = (price + Low) / (High + Low);
            var height = (Factory.Root as RectTransform).rect.height;
            return (percent * height);
        }
    }
}
