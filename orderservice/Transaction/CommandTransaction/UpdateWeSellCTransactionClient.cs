using Microsoft.Extensions.Logging;
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

        public UpdateWeSellCTransactionClient(ILogger<MyLogger> logger, string Name)
        {
            _logger = logger;
        }

        public string Name { get; set; }

        public Dictionary<string, object> Execute(Dictionary<string, object> Input)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> RollBack(Dictionary<string, object> Input)
        {
            throw new NotImplementedException();
        }
    }
}
