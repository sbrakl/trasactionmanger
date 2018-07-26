using Microsoft.Extensions.Logging;
using orderservice.Client;
using orderservice.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.Transaction.CommandTransaction
{
    public class UpdateWeSellCTransactionClient : ICommandTransaction
    {
        private ILogger<MyLogger> _logger;

        public UpdateWeSellCTransactionClient(ILogger<MyLogger> logger, string name)
        {
            _logger = logger;
            Name = name;
        }

        public string Name { get; set; }

        public bool Equals(ICommandTransaction other)
        {
            return this.Name == other.Name;
        }

        public Dictionary<string, object> Execute(Dictionary<string, object> Input)
        {
            Dictionary<string, object> Output = new Dictionary<string, object>();
            var res = CustomerClient.UpdateCustomerTotalSell();
            Output.Add("CustomerMessage", res.Message);
            Output.Add("CustomerKey", res.CustomerKey);
            _logger.LogInformation(res.Message);
            return Output;
        }

        public Dictionary<string, object> RollBack(Dictionary<string, object> Input)
        {
            Dictionary<string, object> Output = new Dictionary<string, object>();
            var res = CustomerClient.RollbackCustomerTotalSell();
            Output.Add("CustomerMessage", res.Message);
            Output.Add("CustomerKey", res.CustomerKey);
            _logger.LogInformation(res.Message);
            return Output;
        }
    }
}
