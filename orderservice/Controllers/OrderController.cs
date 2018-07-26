using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using orderservice.Client;
using orderservice.Logger;
using System.Collections.Generic;
using orderservice.Transaction.BuilderTransaction;
using orderservice.Transaction.CommandTransaction;
using orderservice.Repository;

namespace orderservice.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private ILogger<MyLogger> _logger;
        public OrderController(ILogger<MyLogger> logger)
        {
            _logger = logger;
        }

        [Route("processorder")]
        [HttpGet]
        public ActionResult<string> ProcessOrder()
        {
            BuilderTransactionManager btm = new BuilderTransactionManager(_logger);
            #region Add TransactionClient
            btm.AddTraction(new UpdateStockBTransactionalClient(_logger,
                                                                "stock",
                                                                new Dictionary<string, object>
                                                                {
                                                                    { "Quantity", 5 }
                                                                }
                                                               )
                            );
            btm.AddTraction(new CustomerBTransactionalClient(_logger, "customer"));
            btm.AddTraction(new VoucherBTransactionalClient(_logger, "voucher", new Dictionary<string, object>
                                                            {
                                                                { "Voucher2", "abc" },
                                                                { "OrderNumber", "KR34342" }
                                                            })
                            );
            #endregion
            btm.ExecuteAllInTransaction();
            return string.Format("Order process successfully");
        }

        [Route("processorder2/{ordertype}")]
        [HttpGet]
        public ActionResult<string> ProcessOrder2(string ordertype)
        {
            _logger.LogInformation("ProcessOrder2 called");
            CommandTransactionManager ctm = new CommandTransactionManager(_logger);

            //Update Stock
            UpdateStockCTransactionClient updateStockclient = new UpdateStockCTransactionClient(_logger);
            updateStockclient.Name = "stock";
            var stockoutput = ctm.ExecuteInTransaction(updateStockclient, new Dictionary<string, object>
                                                                {
                                                                    { "Quantity", 5 }
                                                                });


            //Check ordertype
            Dictionary<string, object> customeroutput = null;
            if (ordertype == "buy")
            {
                UpdateWeBuyCTransactionClient updateTotalBuyClient 
                    = new UpdateWeBuyCTransactionClient(_logger, "UpdateCustomerTotalBuy");

                if (!ctm.IsTransactionFailed)
                    customeroutput = ctm.ExecuteInTransaction(updateTotalBuyClient, null);
            }
            else
            {
                UpdateWeSellCTransactionClient updateTotalSellClient
                    = new UpdateWeSellCTransactionClient(_logger, "UpdateCustomerTotalSell");

                if (!ctm.IsTransactionFailed)
                    customeroutput = ctm.ExecuteInTransaction(updateTotalSellClient, null);
            }


            //Do some non tractional service call stuff        
            if (customeroutput != null && !(customeroutput.ContainsKey("CustomerKey")))
            {
                _logger.LogInformation("Non Tractional Call");
                DummyRepo.UpdateOrder();
            }

            //Back again transactional stuff
            VoucherCTransactionClient voucherRedeemClient 
                = new VoucherCTransactionClient(_logger, "VocherRedeemClient");

            //Taking customer key from customer service output
            string customerkey = "";
            if (customeroutput.ContainsKey("CustomerKey"))
                customerkey = (string)customeroutput["CustomerKey"];
            if (!ctm.IsTransactionFailed)
                ctm.ExecuteInTransaction(voucherRedeemClient, new Dictionary<string, object>
                                                            {
                                                                { "Voucher2", "abc" },
                                                                { "OrderNumber", "KR34342" },
                                                                { "CustomerKey", customerkey }
                                                            }, "VoucherRedeeem");

            if (ctm.IsTransactionFailed)
                return "Order processing failed, rollback executed successfully";
            else
                return "Order processed successfully";
        }


    }
}
