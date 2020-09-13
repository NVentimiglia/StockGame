using Framework;
using Stocks.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stocks.Services
{
    /// <summary>
    /// Game Logic
    /// </summary>
    [ExecutionOrder(ExecutionOrderAttribute.SERVICE_2)]
    public class MockStockService : MonoBehaviour, IStockService
    {
        public List<StockBarData> Bars { get; private set; }
        public int Period { get; private set; }
        public TimeSpan PeriodRemaining { get { return TimeSpan.FromSeconds(PeriodLength - delta); } }
        public int BidPrice => Mathf.Max(1, Last - Spread); // Sell Price
        public int AskPrice => Mathf.Max(1, Last + Spread); // Buy Price
        public int Last { get; private set; }
        public int Spread { get; private set; }
        public int High { get; private set; }
        public int Low { get; private set; }
        public int HighVolume { get; private set; }
        public int LowVolume { get; private set; }


        public int StockValue => StockCount * Last;
        public int StockCount { get; private set; }
        public int CashValue { get; private set; }
        public int TotalValue => StockValue + CashValue;

        public bool CanBuy => CashValue >= AskPrice;
        public bool CanSell => StockCount > 0;

        public event Action<StockBarData> OnUpdate = delegate { };

        private float delta;
        private RollingWindow RollingVolume;
        private StockBarData current;
        public float PeriodLength = 30f;

        public void Awake()
        {
            Bars = new List<StockBarData>();
            Period = 0;
            Last = 50;
            StockCount = 0;
            CashValue = 100000;
            Low = 0;
            High = 100;
            LowVolume = 0;
            HighVolume = 10;
            Spread = 1;
            RollingVolume = new RollingWindow(5, 1);
            current = new StockBarData(Last, Period);
            Bars.Add(current);
            OnUpdate(current);
        }

        public bool Buy()
        {
            if (CanBuy)
            {
                var price = AskPrice;
                CashValue -= price;
                StockCount++;
                current.SetPrice(price);
                Last = price;
                High = price > High ? price : High;
                HighVolume = current.Volume > HighVolume ? current.Volume : HighVolume;
                RollingVolume.Add();
                ComputeSpread();
                Spread++;
                OnUpdate(current);
                return true;
            }
            return false;
        }

        public bool Sell()
        {
            if (CanSell)
            {
                var price = BidPrice;
                CashValue += price;
                StockCount--;
                current.SetPrice(price);
                Last = price;
                Low = price < Low ? price : Low;
                LowVolume = current.Volume < LowVolume ? current.Volume : LowVolume;
                RollingVolume.Add();
                ComputeSpread();
                Spread++;
                OnUpdate(current);
                return true;
            }
            return false;
        }

        void NewBar()
        {
            var oldBar = current;
            current = new StockBarData(oldBar.Close, Period);
            Bars.Add(current);
            OnUpdate(current);
        }

        void ComputeSpread()
        {
            var avg = RollingVolume.Average();
            Spread = Mathf.RoundToInt(Mathf.Lerp(1, 10, avg / 100f));
        }

        void Update()
        {
            delta += Time.deltaTime;
            if (delta >= PeriodLength)
            {
                delta = 0;
                Period++;
                Spread = Mathf.Min(Spread - 1, 1);
                RollingVolume.Roll();
                ComputeSpread();
                NewBar();
            }
        }

    }
}
