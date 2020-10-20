using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.Common
{
    public class Currency
    {
        public CurrencyType CurrencyFrom { get; set; }
        public CurrencyType CurrencyTo { get; set; }
        public string URL { get; set; }
        public string XPath { get; set; }
        public decimal? ExchangeRate { get; set; }

        public Currency()
        {
            //
        }
    }
}
