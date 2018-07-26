using Microsoft.Extensions.Logging;
using orderservice.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.Transaction.CommandTransaction
{
    public class CommandTransactionManager
    {
        private ILogger<MyLogger> _logger { get; set; }
        public List<ICommandTransaction> Transactions { get; set; }
        private Dictionary<ICommandTransaction, Dictionary<string, object>> TransactionInputs { get; set; }
        public bool IsTransactionFailed { get; set; }

        public CommandTransactionManager(ILogger<MyLogger> logger)
        {
            _logger = logger;
            Transactions = new List<ICommandTransaction>();
            TransactionInputs = new Dictionary<ICommandTransaction, Dictionary<string, object>>();
            IsTransactionFailed = false;
        }

        public Dictionary<string,object> ExecuteInTransaction(ICommandTransaction client, 
                                                              Dictionary<string, object> Input, 
                                                              string name = "")
        {
            if (IsTransactionFailed)
                throw new ArgumentException("Transactional Manager in error state, create new instance to use it");


            if (String.IsNullOrEmpty(client.Name) && string.IsNullOrEmpty(name))
                throw new ArgumentException("Transactional Client name not passed");
            if (!string.IsNullOrEmpty(name))
                client.Name = name;

            #region Adding client to Dictionary
            if (!Transactions.Contains(client))
            { 
                Transactions.Add(client);
                TransactionInputs.Add(client, Input);
            }
            #endregion

            try
            {
                _logger.LogInformation("Executing client '{0}' in transaction", client.Name);
                var output = client.Execute(Input);
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while executing  client '{0}' in transaction", client.Name);
                _logger.LogError(ex.ToString());
                ExecuteRollBack(client);
                return new Dictionary<string, object> { { "Msg", "Error in Transaction" } };
            }
        }

        private void ExecuteRollBack(ICommandTransaction client)
        {
            _logger.LogWarning("Executing Rollback");
            IsTransactionFailed = true;
            int Index = Transactions.IndexOf(client) - 1;

            for (int index = Index; index >= 0; index--)
            {
                ICommandTransaction trac = Transactions[index];
                Dictionary<string, object> Inputs = TransactionInputs[trac];
                string processname = trac.GetType().Name;
                _logger.LogInformation("{0} transaction rollback executing", processname);
                try
                {                    
                    trac.RollBack(Inputs);
                }
                catch
                {
                    _logger.LogError("Rollback failed for process {0}", processname);
                    _logger.LogError("Transaction in inconsistant state, manual check required");
                    throw;
                }
            }
        }
    }
}
