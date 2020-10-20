using DailyPortfolioTotal.Common;
using DailyPortfolioTotal.DatabaseReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.Util
{
    public class CurrencyConverter
    {
        private List<Currency> currencies;

        public CurrencyConverter(List<Currency> currencies)
        {
            this.currencies = currencies;
        }

        public decimal ConvertCurrency(decimal value, CurrencyType from, CurrencyType to)
        {
            decimal exchangeRate = this.GetExchangeRate(from, to);
            return value * exchangeRate;
        }

        private decimal GetExchangeRate(CurrencyType from, CurrencyType to)
        {
            Currency currency = currencies
                .Where(x => x.CurrencyFrom == from && x.CurrencyTo == x.CurrencyTo)
                .FirstOrDefault();

            if (currency.ExchangeRate == null)
            {
                string exRateStr = WebReader.ReadFromWeb(currency.URL, currency.XPath);
                currency.ExchangeRate = Convert.ToDecimal(exRateStr);
            }

            return currency.ExchangeRate.Value;
        }
    }
}
