using DailyPortfolioTotal.Common;
using DailyPortfolioTotal.Util;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.DatabaseReader
{
    class PriceReader
    {
        private CurrencyConverter currencyConverter;

        public PriceReader(CurrencyConverter currencyConverter)
        {
            this.currencyConverter = currencyConverter;
        }

        public List<Stock> ReadValueOfStocks(List<Stock> stocks)
        {
            DailyPortfolioTotalFunction.Logger.Write("Starting to get stock values...");
            foreach (Stock stock in stocks)
            {
                stock.LatestPricePerShare = this.ReadStockPrice(stock);
            }
            DailyPortfolioTotalFunction.Logger.Write("Finished getting stock values");
            return stocks;
        }

        private decimal ReadStockPrice(Stock stock)
        {
            try
            {
                string priceString = String.Empty;
                priceString = WebReader.ReadFromWeb(stock.URL, stock.XPath, stock.CssSelector);
                priceString = Utility.CleanCurrencyString(priceString);

                decimal priceDecimal = Utility.GetDecimalFromString(priceString, stock.IsScrapedPriceInPence);
                if (stock.Currency != CurrencyType.GBP)
                {
                    priceDecimal = currencyConverter.ConvertCurrency(priceDecimal, stock.Currency, CurrencyType.GBP);
                }
                return priceDecimal;
            }
            catch (Exception e)
            {
                DailyPortfolioTotalFunction.Logger.Write(e.Message + "\n" + e.StackTrace);
                return -1;
            }
        }
    }
}
