using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace voucherservice.Controllers
{
    [Route("voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        [Route("redeemvoucher/{voucher}")]
        [HttpGet]
        public ActionResult<string> RedeemVoucher(string voucher)
        {
            return string.Format("Voucher '{0}' is successfully redeem", voucher) ;
        }
    }
}
