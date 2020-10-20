using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using DailyPortfolioTotal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DailyPortfolioTotal.DatabaseReader
{
    class Dynamo
    {
        private readonly string accessKey = ConfigurationManager.AppSettings["DynamoAccessKey"];
        private readonly string secretKey = ConfigurationManager.AppSettings["DynamoSecret"];
        private BasicAWSCredentials credentials;
        
        public AmazonDynamoDBClient Client;

        public Dynamo()
        {
            credentials = new BasicAWSCredentials(accessKey, secretKey);
            Client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
        }
    }
}
