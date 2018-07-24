using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace customerservice.Controllers
{
    [Route("customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [Route("UpdateCustomer/{customerId}")]
        [HttpGet]
        public ActionResult<string> UpdateCustomer(string customerId)
        {
            return string.Format("Customer {0} Succcessfully updated", customerId);
        }
             
    }
}
