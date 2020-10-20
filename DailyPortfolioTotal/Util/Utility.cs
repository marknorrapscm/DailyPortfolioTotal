using DailyPortfolioTotal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.Util
{
    class Utility
    {
        public static CurrencyType GetCurrencyTypeFromString(string str)
        {
            switch (str)
            {
                case "GBP": { return CurrencyType.GBP; }
                case "USD": { return CurrencyType.USD; }
                case "CAD": { return CurrencyType.CAD; }
                case "JPY": { return CurrencyType.JPY; }
                default: { return CurrencyType.GBP; }
            }
        }

        public static string CleanCurrencyString(string str)
        {
            return str.Replace("GBX", "")
                .Replace("GBP", "")
                .Replace("USD", "")
                .Replace("CAD", "")
                .Replace("EUR", "")
                .Replace("£", "")
                .Replace("$", "")
                .Replace("€", "")
                .Replace(" ", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("p", "")
                .Replace(",", "")
                .Replace("-", "")
                .Replace("<", "")
                .Replace(">", "");
        }

        public static decimal GetDecimalFromString(string str, bool isBuyPriceInPence)
        {
            decimal priceDecimal = -1;
            Decimal.TryParse(str, out priceDecimal);

            if (isBuyPriceInPence)
            {
                priceDecimal = priceDecimal / 100;
            }
            return priceDecimal;
        }
    }
}
