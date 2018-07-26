using Microsoft.Extensions.Logging;
using orderservice.Client;
using orderservice.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.BuilderTransaction
{
    public class UpdateStockTransactionalClient : IBuilderTransaction
    {
        private ILogger<MyLogger> _logger;

        public UpdateStockTransactionalClient(ILogger<MyLogger> logger, string name, Dictionary<string, object> input)
        {
            Name = name;
            this.Input = input;
            _logger = logger;
            this.Output = new Dictionary<string, object>();
        }

        public string Name { get; set; }
        public Dictionary<string, object> Input { get; set; }
        public Dictionary<string, object> Output { get; set; }

        public void Execute()
        {
            if (!Input.ContainsKey("Quantity"))
                throw new ArgumentException("Quantity not passed");
            var quantity = (int)Input["Quantity"];
            var msg = StockClient.StockUpdate(quantity);
            Output.Add("StockMessage", msg);
            _logger.LogInformation(msg);
        }

        public void RollBack()
        {
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
        }
    }
}
