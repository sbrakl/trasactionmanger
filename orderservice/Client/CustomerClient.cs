using Newtonsoft.Json;
using RestSharp;
using System;

namespace orderservice.Client
{
    public class CustomerClient
    {
        public static CustomerResponse UpdateCustomerTotalBuy()
        {
            var customerclient = new RestClient("http://localhost:5002");
            var customerrequest = new RestRequest("customer/updatebuytotal/5", Method.GET);
            customerclient.Timeout = 2000;
            IRestResponse response = customerclient.Execute(customerrequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Customer service failed");
            CustomerResponse res = JsonConvert.DeserializeObject<CustomerResponse>(response.Content);
            return res;
        }

        public static CustomerResponse RollbackCustomerTotalBuy()
        {
            var customerclient = new RestClient("http://localhost:5002");
            var customerrequest = new RestRequest("customer/rollbackbuytotal/5", Method.GET);
            customerclient.Timeout = 2000;
            IRestResponse response = customerclient.Execute(customerrequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Customer service failed");            
            CustomerResponse res = JsonConvert.DeserializeObject<CustomerResponse>(response.Content);
            return res;
        }

        public static CustomerResponse UpdateCustomerTotalSell()
        {
            var customerclient = new RestClient("http://localhost:5002");
            var customerrequest = new RestRequest("customer/updateselltotal/5", Method.GET);
            customerclient.Timeout = 2000;
            IRestResponse response = customerclient.Execute(customerrequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Customer service failed");
            CustomerResponse res = JsonConvert.DeserializeObject<CustomerResponse>(response.Content);
            return res;
        }

        public static CustomerResponse RollbackCustomerTotalSell()
        {
            var customerclient = new RestClient("http://localhost:5002");
            var customerrequest = new RestRequest("customer/rollbackselltotal/5", Method.GET);
            customerclient.Timeout = 2000;
            IRestResponse response = customerclient.Execute(customerrequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Customer service failed");
            CustomerResponse res = JsonConvert.DeserializeObject<CustomerResponse>(response.Content);
            return res;
        }
    }

    public class CustomerResponse
    {
        public string Message { get; set; }
        public string CustomerKey { get; set; }
    }
}
