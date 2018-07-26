using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.Transaction.CommandTransaction
{
    public interface ICommandTransaction
    {
        string Name { get; set; }         
        Dictionary<string, object> Execute(Dictionary<string, object> Input);
        Dictionary<string, object> RollBack(Dictionary<string, object> Input);
    }
}
