using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;
using DailyPortfolioTotal.Common;
using DailyPortfolioTotal.DatabaseReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.Writer
{
    public class DynamoWriter
    {
        private Dynamo dynamo;

        public DynamoWriter()
        {
            this.dynamo = new Dynamo();
        }

        public void WriteStockValues(List<Stock> stocks)
        {
            foreach (var stock in stocks)
            {
                var updateRequest = CreateUpdateDailyShareValueRequest(stock.Sedol, stock.ShareName, stock.TotalValue);

                dynamo.Client.UpdateItem(updateRequest);
            }
        }

        public void WriteDailyTotal(decimal dailyShareTotal)
        {
            var updateRequest = CreateUpdateDailyTotalRequest("1", dailyShareTotal);

            dynamo.Client.UpdateItem(updateRequest);
        }

        private UpdateItemRequest CreateUpdateDailyShareValueRequest(string sedol, string shareName, decimal value)
        {
            return new UpdateItemRequest
            {
                TableName = "DailyShareValue",
                Key = new Dictionary<string, AttributeValue> { { "Sedol", new AttributeValue { S = sedol } } },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#Values", "Values" },
                    { "#ShareName", "ShareName" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":sharename", new AttributeValue { S = shareName } },
                    {
                        ":newvalue",
                        new AttributeValue
                        {
                            L = new List<AttributeValue>()
                            {
                                {
                                    new AttributeValue
                                    {
                                        M = new Dictionary<string, AttributeValue>()
                                        {
                                            { "Date", new AttributeValue { S = DateTime.Now.ToString("yyyy-MM-dd") } },
                                            { "Value", new AttributeValue { N = value.ToString() } }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                UpdateExpression = "SET #Values = list_append(if_not_exists(#Values, :newvalue), :newvalue), " +
                                       "#ShareName = if_not_exists(#ShareName, :sharename)"
            };
        }

        private UpdateItemRequest CreateUpdateDailyTotalRequest(string id, decimal value)
        {
            return new UpdateItemRequest
            {
                TableName = "DailyTotal",
                Key = new Dictionary<string, AttributeValue> { { "ID", new AttributeValue { S = id } } },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#Values", "Values" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":newvalue",
                        new AttributeValue
                        {
                            L = new List<AttributeValue>()
                            {
                                {
                                    new AttributeValue
                                    {
                                        M = new Dictionary<string, AttributeValue>()
                                        {
                                            { "Date", new AttributeValue { S = DateTime.Now.ToString("yyyy-MM-dd") } },
                                            { "Value", new AttributeValue { N = value.ToString() } }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                UpdateExpression = "SET #Values = list_append(if_not_exists(#Values, :newvalue), :newvalue)"
            };
        }

    }
}
