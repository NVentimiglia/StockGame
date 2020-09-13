using Framework;
using Stocks.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stocks.Views
{
    [Serializable]
    public class ObservableStockBarData : ObservableStream<StockBarData> { }

    /// <summary>
    /// View Entry Point
    /// </summary>
    [ExecutionOrder(ExecutionOrderAttribute.DEFAULT_4)]
    public class StockViewModel : MonoBehaviour, IStream<StockBarData>
    {
        private IStockService Service;

        public ObservableString Name;

        public ObservableInt CashValue;
        public ObservableInt Period;
        public ObservableTimeSpan PeriodRemaining;
        public ObservableInt StockCount;
        public ObservableInt StockValue;
        public ObservableInt TotalValue;
        public ObservableInt High;
        public ObservableInt Low;
        public ObservableInt HighVolume;
        public ObservableInt LowVolume;
        public ObservableInt BidPrice;
        public ObservableInt AskPrice;
        public ObservableBool CanBuy;
        public ObservableBool CanSell;
        public ObservableStockBarData StockData;

        private void Awake()
        {
            Service = DependencyService.Get<IStockService>();
            Name.Set("/ES");
            StockData.Set(this);
        }

        private void Update()
        {
            CashValue.Set(Service.CashValue);
            StockCount.Set(Service.StockCount);
            StockValue.Set(Service.StockValue);
            TotalValue.Set(Service.TotalValue);
            BidPrice.Set(Service.BidPrice);
            AskPrice.Set(Service.AskPrice);
            CanBuy.Set(Service.CanBuy);
            CanSell.Set(Service.CanSell);
            High.Set(Service.High);
            Low.Set(Service.Low);
            HighVolume.Set(Service.HighVolume);
            LowVolume.Set(Service.LowVolume);
            PeriodRemaining.Set(Service.PeriodRemaining);
            Period.Set(Service.Period);
        }

        public void Buy()
        {
            Service.Buy();
        }

        public void Sell()
        {
            Service.Sell();
        }

        public IEnumerable<StockBarData> Get()
        {
            return Service.Bars;
        }

        public void Subscribe(Action<StockBarData> handler)
        {
            Service.OnUpdate += handler;
        }

        public void Unsubscribe(Action<StockBarData> handler)
        {
            Service.OnUpdate -= handler;
        }
    }
}
