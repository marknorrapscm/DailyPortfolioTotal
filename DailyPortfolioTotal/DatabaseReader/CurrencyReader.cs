using Amazon.DynamoDBv2.Model;
using DailyPortfolioTotal.Common;
using DailyPortfolioTotal.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.DatabaseReader
{
    class CurrencyReader
    {
        private Dynamo dynamoReader;

        public CurrencyReader()
        {
            this.dynamoReader = new Dynamo();
        }

        public List<Currency> ReadCurrencies()
        {
            var response = dynamoReader.Client.Scan(new ScanRequest("Currency"));
            var currencies = new List<Currency>();

            foreach (var item in response.Items)
            {
                var currency = ConvertToCurrency(item);
                currencies.Add(currency);
            }

            return currencies;
        }

        private Currency ConvertToCurrency(Dictionary<string, AttributeValue> rawItemFromDynamo)
        {
            var currency = new Currency();
            currency.CurrencyFrom = Utility.GetCurrencyTypeFromString(rawItemFromDynamo["CurrencyFrom"].S);
            currency.CurrencyTo = Utility.GetCurrencyTypeFromString(rawItemFromDynamo["CurrencyTo"].S);
            currency.URL = rawItemFromDynamo["URL"].S;
            currency.XPath = rawItemFromDynamo["XPath"].S;

            return currency;
        }
    }
}
