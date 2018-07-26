using Microsoft.Extensions.Logging;
using orderservice.Client;
using orderservice.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.Transaction.CommandTransaction
{
    public class VoucherCTransactionClient : ICommandTransaction
    {
        private ILogger<MyLogger> _logger;

        public VoucherCTransactionClient(ILogger<MyLogger> logger, string Name)
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
            if (!Input.ContainsKey("Voucher"))
                throw new ArgumentException("Voucher not passed");
            if (!Input.ContainsKey("OrderNumber"))
                throw new ArgumentException("OrderNumber not passed");
            if (!Input.ContainsKey("CustomerKey"))
                throw new ArgumentException("CustomerKey not passed");
            string vchr = (string)Input["Voucher"];
            string customerkey = (string)Input["CustomerKey"];
            var msg = VoucherClient.RedeeemVoucher(vchr);
            Output.Add("VoucherMessage", msg);
            _logger.LogInformation(msg);
            return Output;
        }

        public Dictionary<string, object> RollBack(Dictionary<string, object> Input)
        {
            Dictionary<string, object> Output = new Dictionary<string, object>();
            string vchr = (string)Input["Voucher"];
            var msg = VoucherClient.UnRedeeemVoucher(vchr);
            Output.Add("VoucherMessage", msg);
            return Output;
        }
    }
}
