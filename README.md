## DailyPortfolioFunction

### 🚀 What is it?

This is an Azure Function written in C# for the .NET Framework v4.7.2 that does the following:

-   Uses the AWS-SDK for .NET to connect to a DynamoDB (AWS's implementation of a document DB)
-   Reads a list of shares from that database
-   Scrapes various websites to get the current price for each of those shares
-   Writes those values back to DynamoDB, thus creating a daily record of the share prices

### 🎓 Why?

The purpose is to track the value of my shares so they can be graphed / have other metrics calculated on a frontend of my choosing. The problem with online brokers is that they only give you the price you paid and the current value.

### ☁️ Why are both Azure and AWS being used?

The entire app was originally written in Azure when I had \$150 per month of Azure credit; that meant I could use a SQL Server database and it didn't cost me anything. However, now that I no longer have that I have opted to take advantage of the free DynamoDB offering from AWS (there is no permanently free RDS offered by any of the major cloud providers). The original Azure Function remains, the only change being that it now targets a DynamoDB rather than a SQL Server instance.

---

<sub>\*This app is the <i>DailyShareTotal</i> Azure function app in the diagram below:

[<img src="https://i.imgur.com/lNdKXiO.png">](https://i.imgur.com/lNdKXiO.png)
