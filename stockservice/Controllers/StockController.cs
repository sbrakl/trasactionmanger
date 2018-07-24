using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace stockservice.Controllers
{
    [Route("stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        [Route("adjuststock/{quantity}")]
        [HttpGet]
        public ActionResult<string> AdjustStock(int quantity)
        {
            return string.Format("Stock adjust with Quantity {0}", quantity);
        }
    }
}
