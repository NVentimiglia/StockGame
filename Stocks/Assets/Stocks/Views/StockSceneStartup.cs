using Framework;
using Stocks.Models;
using Stocks.Services;
using UnityEngine;

namespace Stocks.Views
{
    /// <summary>
    /// Game Entry Point
    /// </summary>
    [ExecutionOrder(ExecutionOrderAttribute.CONTROLER_3)]
    public class StockSceneStartup : MonoBehaviour
    {
        private void Awake()
        {
            DependencyService.Add<IStockService>(GetComponent<MockStockService>());
        }
    }
}
