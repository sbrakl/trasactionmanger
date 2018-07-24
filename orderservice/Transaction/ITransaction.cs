using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.Transaction
{
    public interface ITransaction
    {
        string Name { get; set; }
        Dictionary<string, object> Input { get; set; }
        Dictionary<string, object> Output { get; set; }
        void Execute();
        void RollBack();
    }
}
