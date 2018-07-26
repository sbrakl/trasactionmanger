using Microsoft.Extensions.Logging;
using orderservice.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.BuilderTransaction
{
    /// <summary>
    /// Transactional Manager based on Builder Pattern
    /// </summary>
    public class BuilderTransactionManager
    {
        public List<ITransaction> Transactions { get; set; }
        private ILogger<MyLogger> _logger { get; set; }

        private Dictionary<string, object> TransactionLevelOutput { get; set; }

        public BuilderTransactionManager(ILogger<MyLogger> logger)
        {
            _logger = logger;
            Transactions = new List<ITransaction>();
            TransactionLevelOutput = new Dictionary<string, object>();
        }

        public BuilderTransactionManager(ILogger<MyLogger> logger, List<ITransaction> transactions)
        {
            _logger = logger;
            Transactions = transactions;
            TransactionLevelOutput = new Dictionary<string, object>();
        }

        public void AddTraction(ITransaction trac)
        {
            Transactions.Add(trac);
        }

        public void ExecuteAllInTransaction()
        {
            _logger.LogInformation("Transaction Manager is starting the traction");
            int index = 0;
            bool isRollbackOccur = false;
            for (index = 0; index < Transactions.Count; index++)
            {
                ITransaction trac = Transactions[index];
                string processname = trac.Name;
                _logger.LogInformation("{0} transaction executing", processname);
                try
                {
                    AddTransactionLevelOutputToInput(trac);
                    trac.Execute();
                    AddOutputToTransactionLevelOutput(trac);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    isRollbackOccur = true;
                    ExecuteRollBack(index-1);
                    break;
                }
            }
            if (!isRollbackOccur)
                _logger.LogInformation("Transaction completed successfully");
            else
                _logger.LogInformation("Transaction completed with Rollback");
        }

        private void ExecuteRollBack(int Index)
        {
            for (int index = Index; index >= 0; index--)
            {
                ITransaction trac = Transactions[index];
                string processname = trac.GetType().Name;
                _logger.LogInformation("{0} transaction rollback executing", processname);
                try
                {
                    AddTransactionLevelOutputToInput(trac);
                    trac.RollBack();
                    AddOutputToTransactionLevelOutput(trac);
                }
                catch
                {
                    _logger.LogError("Rollback failed for process {0}", processname);
                    _logger.LogError("Transaction in inconsistant state, manual check required");
                    throw;
                }
            }
        }

        private void AddTransactionLevelOutputToInput(ITransaction trac)
        {
            var tracInput = trac.Input;
            if (tracInput == null)
                return;
            foreach (KeyValuePair<string, object> kv in TransactionLevelOutput)
            {
                if (tracInput.ContainsKey(kv.Key))
                    tracInput[kv.Key] = kv.Value;
                else
                    tracInput.Add(kv.Key, kv.Value);
            }
        }

        private void AddOutputToTransactionLevelOutput(ITransaction trac)
        {
            var tracOutput = trac.Output;
            foreach (KeyValuePair<string, object> kv in tracOutput)
            {
                if (TransactionLevelOutput.ContainsKey(kv.Key))
                    TransactionLevelOutput[kv.Key] = kv.Value;
                else
                    TransactionLevelOutput.Add(kv.Key, kv.Value);
            }
        }

        

    }
}