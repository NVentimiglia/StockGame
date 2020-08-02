using Framework;
using System;
using System.Collections.Generic;

namespace Stocks.Models
{
    [Serializable]
    public class StockBarData : IEquatable<StockBarData>, IComparable<StockBarData>
    {
        public int Period;

        public int High;
        public int Low;

        public int Open;
        public int Close;
        public int Volume;

        public bool Red;

        public int CompareTo(StockBarData other)
        {
            return Period.CompareTo(other.Period);
        }

        public bool Equals(StockBarData other)
        {
            return Period.Equals(other.Period);
        }

        public StockBarData(int price = 0, int period = 0)
        {
            Period = period;
            Low = price;
            High = price;
            Open = price;
            Close = price;
            Red = Open > Close;
        }

        public void SetPrice(int price)
        {
            if (price < Low)
                Low = price;
            if (price > High)
                High = price;

            if (Open == 0)
                Open = price;

            Close = price;
            Volume++;

            Red = Open > Close;
        }
    }

    public interface IStockService
    {
        // History
        List<StockBarData> Bars { get; }
        int Period { get; }
        TimeSpan PeriodRemaining { get; }

        // Quotes
        int BidPrice { get; }
        int AskPrice { get; }
        int Last { get; }
        int Spread { get; }
        int High { get; }
        int Low { get; }
        int HighVolume { get; }
        int LowVolume { get; }

        //Account
        int StockCount { get; }
        int StockValue { get; }
        int CashValue { get; }
        int TotalValue { get; }
        bool CanBuy { get; }
        bool CanSell { get; }

        // Events
        event Action<StockBarData> OnUpdate;

        // Commands
        bool Buy();
        bool Sell();
    }
}
