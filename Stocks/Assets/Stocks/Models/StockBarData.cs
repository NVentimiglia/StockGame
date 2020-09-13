using System;

namespace Stocks.Models
{
    /// <summary>
    /// Data Model
    /// </summary>
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
}
