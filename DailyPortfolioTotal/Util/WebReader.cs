using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.Util
{
    class WebReader
    {
        public static string ReadFromWeb(string url, string xpath, string cssSelector = "")
        {
            // Enable all possible versions of HTTPS (required for Charles Stanley)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Ssl3;

            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument page = web.Load(url);
                    HtmlNode xpathNode = page.DocumentNode.SelectSingleNode(xpath);

                    if (xpathNode != null)
                    {
                        return xpathNode.InnerText;
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(cssSelector))
                        {
                            HtmlNode cssSelectorNode = page.QuerySelector(cssSelector);
                            return cssSelectorNode.InnerText;
                        }
                    }
                }
                catch (Exception e)
                {
                    DailyPortfolioTotalFunction.Logger.Write(String.Format("Failed to read URL on attempt #{0} of 3: \nURL: {1}\nXPath: {2}\nError:{3}\n{4}\nCssSelector:{5}",
                                                             attempt + 1,
                                                             url,
                                                             xpath,
                                                             e.Message,
                                                             e.StackTrace,
                                                             cssSelector));
                }
            }
            throw new Exception("Failed to read URL after 3 attempts");
        }
    }
}
