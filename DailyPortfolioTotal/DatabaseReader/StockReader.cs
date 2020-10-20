using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using DailyPortfolioTotal.Common;
using DailyPortfolioTotal.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.DatabaseReader
{
    class StockReader
    {
        private Dynamo dynamo;

        public StockReader()
        {
            this.dynamo = new Dynamo();
        }

        public List<Stock> ReadShares()
        {
            var request = CreateGetStocksRequest();
            var response = dynamo.Client.Scan(request);
            var stocks = ReadStocksFromResponse(response);

            return stocks;
        }

        private List<Stock> ReadStocksFromResponse(ScanResponse response)
        {
            var stocks = new List<Stock>();

            foreach (var item in response.Items)
            {
                var stock = ConvertToStock(item);

                var existingStock = stocks.Where(x => x.Sedol == stock.Sedol).FirstOrDefault();
                if (existingStock == null)
                {
                    stocks.Add(stock);
                }
                else
                {
                    existingStock.Quantity += stock.Quantity;
                }
            }

            return stocks;
        }

        private ScanRequest CreateGetStocksRequest()
        {
            Dictionary<string, Condition> conditions = new Dictionary<string, Condition>();

            Condition hasBeenSoldCondition = new Condition();
            hasBeenSoldCondition.ComparisonOperator = ComparisonOperator.EQ;
            hasBeenSoldCondition.AttributeValueList.Add(new AttributeValue { BOOL = false });
            conditions["HasBeenSold"] = hasBeenSoldCondition;

            ScanRequest request = new ScanRequest();
            request.TableName = "Shares";
            request.ScanFilter = conditions;

            return request;
        }

        private Stock ConvertToStock(Dictionary<string, AttributeValue> rawItemFromDynamo)
        {
            var stock = new Stock();
            stock.CssSelector = rawItemFromDynamo["CssSelector"].S;
            stock.Currency = Utility.GetCurrencyTypeFromString(rawItemFromDynamo["Currency"].S);
            stock.Type = rawItemFromDynamo["IsFund"].BOOL ? StockType.Fund : StockType.Share;
            stock.IsScrapedPriceInPence = rawItemFromDynamo["IsScrapedPriceInPence"].BOOL;
            stock.Quantity = Convert.ToDecimal(rawItemFromDynamo["Quantity"].N);
            stock.Sedol = rawItemFromDynamo["Sedol"].S;
            stock.ShareName = rawItemFromDynamo["ShareName"].S;
            stock.URL = rawItemFromDynamo["URL"].S;
            stock.XPath = rawItemFromDynamo["XPath"].S;
            stock.BuyPrice = Convert.ToDecimal(rawItemFromDynamo["BuyPrice"].N);

            return stock;
        }
    }
}
