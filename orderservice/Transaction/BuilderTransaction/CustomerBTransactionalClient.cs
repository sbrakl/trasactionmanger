using Microsoft.Extensions.Logging;
using orderservice.Client;
using orderservice.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.Transaction.BuilderTransaction
{
    public class CustomerBTransactionalClient : IBuilderTransaction
    {
        private ILogger<MyLogger> _logger;

        public CustomerBTransactionalClient(ILogger<MyLogger> logger, string name)
        {
            Name = name;
            _logger = logger;
            this.Output = new Dictionary<string, object>();
        }

        public string Name { get; set; }
        public Dictionary<string, object> Input { get; set; }
        public Dictionary<string, object> Output { get; set; }


        public void Execute()
        {            
            var res = CustomerClient.UpdateCustomerSaleNumbers();
            Output.Add("CustomerMessage", res.Message);
            Output.Add("CustomerKey", res.CustomerKey);
            _logger.LogInformation(res.Message);            
        }

        public void RollBack()
        {                     
            var res = CustomerClient.RollbackUpdateCustomerSaleNumbers();
            if (Output.ContainsKey("CustomerMessage"))
                Output["Output"] = res.Message;
            else
                Output.Add("CustomerMessage", res.Message);

            if (Output.ContainsKey("CustomerKey"))
                Output["CustomerKey"] = res.CustomerKey;
            else
                Output.Add("CustomerKey", res.CustomerKey);

            _logger.LogInformation(res.Message);
        }
    }
}
