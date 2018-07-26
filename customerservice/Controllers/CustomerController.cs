using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using customerservice.Model;
using Microsoft.AspNetCore.Mvc;

namespace customerservice.Controllers
{
    [Route("customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [Route("updatebuytotal/{customerId}")]
        [HttpGet]
        public ActionResult<CustomerResponse> UpdateCustomerBuyTotal(string customerId)
        {
            return new CustomerResponse
            {
                CustomerKey = "KR" + customerId,
                Message = string.Format("Customer {0} BUY total succcessfully updated", customerId),
                UpdateTotal = 100
            };
        }

        [Route("rollbackbuytotal/{customerId}")]
        [HttpGet]
        public ActionResult<CustomerResponse> RollbackCustomerBuyTotal(string customerId)
        {
            return new CustomerResponse
            {
                CustomerKey = "KR" + customerId,
                Message = string.Format("Customer {0} BUY total succcessfully rollback", customerId),
                UpdateTotal = 90
            };
        }

        [Route("updateselltotal/{customerId}")]
        [HttpGet]
        public ActionResult<CustomerResponse> UpdateCustomerSellTotal(string customerId)
        {
            return new CustomerResponse
            {
                CustomerKey = "KR" + customerId,
                Message = string.Format("Customer {0} SELL totaL succcessfully updated", customerId),
                UpdateTotal = 30
            };
        }

        [Route("rollbackselltotal/{customerId}")]
        [HttpGet]
        public ActionResult<CustomerResponse> RollbackCustomerSellTotal(string customerId)
        {
            return new CustomerResponse
            {
                CustomerKey = "KR" + customerId,
                Message = string.Format("Customer {0} SELL totaL succcessfully rollback", customerId),
                UpdateTotal = 25
            };
        }
    }
}

