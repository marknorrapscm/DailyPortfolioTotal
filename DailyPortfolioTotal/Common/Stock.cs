using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.Common
{
    public class Stock
    {
        public string ShareName { get; set; }
        public string Sedol { get; set; }
        public StockType Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal LatestPricePerShare { get; set; }
        public bool IsScrapedPriceInPence { get; set; }
        public string URL { get; set; }
        public string XPath { get; set; }
        public string CssSelector { get; set; }
        public CurrencyType Currency { get; set; }
        public decimal TotalValue
        {
            get
            {
                return this.Quantity * this.LatestPricePerShare;
            }
        }
        public decimal BuyPrice { get; set; }

        public Stock()
        {
            //
        }
    }
}
