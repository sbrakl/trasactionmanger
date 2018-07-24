using RestSharp;
using System;

namespace orderservice.Client
{
    public class CustomerClient
    {
        public static CustomerResponse UpdateCustomerSaleNumbers()
        {
            var customerclient = new RestClient("http://localhost:5002");
            var customerrequest = new RestRequest("customer/updatecustomer/5", Method.GET);
            customerclient.Timeout = 2000;
            IRestResponse response = customerclient.Execute(customerrequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Customer service failed");
            var content = response.Content;
            return new CustomerResponse
            {
                Message = content,
                CustomerKey = "someKey"
            };
        }

        public static CustomerResponse RollbackUpdateCustomerSaleNumbers()
        {
            var customerclient = new RestClient("http://localhost:5002");
            var customerrequest = new RestRequest("customer/updatecustomer/5", Method.GET);
            customerclient.Timeout = 2000;
            IRestResponse response = customerclient.Execute(customerrequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Customer service failed");
            var content = response.Content;
            return new CustomerResponse
            {
                Message = content                
            };
        }
    }

    public class CustomerResponse
    {
        public string Message { get; set; }
        public string CustomerKey { get; set; }
    }
}
