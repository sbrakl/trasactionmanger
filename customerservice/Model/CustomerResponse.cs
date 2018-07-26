using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customerservice.Model
{
    public class CustomerResponse
    {
        public string Message { get; set; }
        public string CustomerKey { get; set; }
        public int UpdateTotal { get; set; }
    }
}
