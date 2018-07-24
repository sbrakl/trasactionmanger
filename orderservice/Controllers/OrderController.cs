using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using orderservice.Client;
using orderservice.Logger;
using orderservice.Transaction;
using System.Collections.Generic;

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
            KiwiTransactionManager ktm = new KiwiTransactionManager(_logger);
            #region Add TransactionClient
            ktm.AddTraction(new UpdateStockTransactionalClient(_logger,
                                                                "stock",
                                                                new Dictionary<string, object>
                                                                {
                                                                    { "Quantity", 5 }
                                                                }
                                                               )
                            );
            ktm.AddTraction(new CustomerTransactionalClient(_logger, "customer"));
            ktm.AddTraction(new VoucherTransactionalClient(_logger, "voucher", new Dictionary<string, object>
                                                            {
                                                                { "Voucher2", "abc" },
                                                                { "OrderNumber", "KR34342" }
                                                            })
                            );
            #endregion
            ktm.ExecuteAllInTransaction();
            return string.Format("Order process successfully");
        }

        
    }
}
