using Framework;
using TMPro;
using UnityEngine;

namespace Stocks.Views
{
    /// <summary>
    /// Price Grid Item
    /// </summary>
    [ExecutionOrder(ExecutionOrderAttribute.DEFAULT_4)]
    public class StockPriceGridRow : MonoBehaviour
    {
        public TextMeshProUGUI Label;
    }
}
