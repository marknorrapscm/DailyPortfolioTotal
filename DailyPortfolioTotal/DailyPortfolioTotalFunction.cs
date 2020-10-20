using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Configuration;
using DailyPortfolioTotal.DatabaseReader;
using DailyPortfolioTotal.Common;
using System.Collections.Generic;
using DailyPortfolioTotal.Writer;
using DailyPortfolioTotal.Util;

namespace DailyPortfolioTotal
{
    public static class DailyPortfolioTotalFunction
    {
        public static Logger Logger;

        [FunctionName("DailyPortfolioTotalFunction")]
        public static void Run([TimerTrigger("0 0 22 * * *", RunOnStartup = false)]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                Logger = new Logger(log);
                Logger.Write("Starting new process");

                StockReader stockReader = new StockReader();
                List<Stock> stocks = stockReader.ReadShares();

                CurrencyReader currencyReader = new CurrencyReader();
                List<Currency> currencies = currencyReader.ReadCurrencies();
                CurrencyConverter currencyConverter = new CurrencyConverter(currencies);

                PriceReader priceReader = new PriceReader(currencyConverter);
                stocks = priceReader.ReadValueOfStocks(stocks);

                DynamoWriter stockWriter = new DynamoWriter();
                stockWriter.WriteStockValues(stocks);
                stockWriter.WriteDailyTotal(stocks.Sum(x => x.TotalValue));

                Logger.Write("Process complete");
            }
            catch (Exception e)
            {
                log.Info(String.Format("Exception thrown: {0} - {1}", e.Message, e.StackTrace));
            }
        }
    }
}
