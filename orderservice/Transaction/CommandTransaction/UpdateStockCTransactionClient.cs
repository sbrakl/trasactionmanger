using Microsoft.Extensions.Logging;
using orderservice.Client;
using orderservice.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.Transaction.CommandTransaction
{
    public class UpdateStockCTransactionClient : ICommandTransaction
    {
        private ILogger<MyLogger> _logger;

        public UpdateStockCTransactionClient(ILogger<MyLogger> logger)
        {
            _logger = logger;
        }

        public string Name { get; set; }

        public bool Equals(ICommandTransaction other)
        {
            return this.Name == other.Name;
        }

        public Dictionary<string, object> Execute(Dictionary<string, object> Input)
        {
            Dictionary<string, object> Output = new Dictionary<string, object>();
            if (!Input.ContainsKey("Quantity"))
                throw new ArgumentException("Quantity not passed");
            var quantity = (int)Input["Quantity"];
            var msg = StockClient.StockUpdate(quantity);
            Output.Add("StockMessage", msg);            
            _logger.LogInformation(msg);
            return Output;
        }

        public Dictionary<string, object> RollBack(Dictionary<string, object> Input)
        {
            Dictionary<string, object> Output = new Dictionary<string, object>();
            if (!Input.ContainsKey("Quantity"))
                throw new ArgumentException("Quantity not passed");
            var quantity = (int)Input["Quantity"];
            int rollbackqty = quantity * -1;
            var msg = StockClient.StockUpdate(rollbackqty);

            if (Output.ContainsKey("StockMessage"))
                Output["StockMessage"] = msg;
            else
                Output.Add("StockMessage", msg);

            _logger.LogInformation(msg);
            return Output;
        }
    }
}
