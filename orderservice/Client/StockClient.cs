using RestSharp;
using System;

namespace orderservice.Client
{
    public class StockClient
    {
        public static string StockUpdate(int quantity)
        {
            var stockclient = new RestClient("http://localhost:5003");
            var stockrequest = new RestRequest("stock/adjuststock/{quantity}", Method.GET);
            stockrequest.AddUrlSegment("quantity", quantity.ToString());
            stockclient.Timeout = 2000;
            IRestResponse response = stockclient.Execute(stockrequest);
            if (response.StatusCode == 0)
                throw new Exception("Stock service has timeout");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Stock service failed");
            if (!string.IsNullOrEmpty(response.ErrorMessage))
                throw new Exception(response.ErrorMessage, response.ErrorException);
            
            var content = response.Content;
            return content;
        }     
    }
}
