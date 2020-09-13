using System;
using System.Collections.Generic;

namespace Stocks.Models
{
    /// <summary>
    /// Data Service
    /// </summary>
    /// <remarks>
    /// This service should  be chopped up into multiple services / models
    /// I did not for simplicity sake.
    /// </remarks>
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
