using Microsoft.Extensions.Logging;
using orderservice.Client;
using orderservice.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.BuilderTransaction
{
    public class VoucherTransactionalClient : IBuilderTransaction
    {
        private ILogger<MyLogger> _logger;

        public string Name { get; set; }
        public Dictionary<string, object> Input { get; set; }
        public Dictionary<string, object> Output { get; set; }


        public VoucherTransactionalClient(ILogger<MyLogger> logger, string name)
        {
            Name = name;
            _logger = logger;
            Input = new Dictionary<string, object>();
            Output = new Dictionary<string, object>();
        }

        public VoucherTransactionalClient(ILogger<MyLogger> logger, string name, Dictionary<string, object> inputs)
        {
            Name = name;
            _logger = logger;
            Input = inputs;
            Output = new Dictionary<string, object>();
        }     

        public void Execute()
        {            
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
        }

        public void RollBack()
        {            
            if (!Input.ContainsKey("Voucher"))
                throw new ArgumentException("Voucher not passed");
            string vchr = (string)Input["Voucher"];
            var msg = VoucherClient.UnRedeeemVoucher(vchr);
            Output.Add("VoucherMessage", msg);
        }
    }
}
