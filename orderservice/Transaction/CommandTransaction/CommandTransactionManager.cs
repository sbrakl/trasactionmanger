﻿using Microsoft.Extensions.Logging;
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

        public Dictionary<string, object> ExecuteInTransaction(ICommandTransaction client,
                                                              Dictionary<string, object> Input,
                                                              string name = "")
        {
            if (IsTransactionFailed)
                throw new InconsistantStateException("Transactional Manager in error state, create a new instance to use it");


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
                throw new TransactionExecutionException("Transaction failed and rollback executed successfully");
            }
        }

        private void ExecuteRollBack(ICommandTransaction client)
        {
            _logger.LogWarning("Executing Implicit rollback");
            if (IsTransactionFailed)
                throw new InconsistantStateException("Transactional Manager in error state, It would be previously Rollback have been executed. Check logs for more detail");

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
                catch(Exception ex)
                {
                    _logger.LogError("Rollback failed for process {0}", processname);
                    _logger.LogError("Transaction in inconsistant state, manual check required");
                    throw new RollbackFailedException("Rollbacked filed for transaction " + processname, ex);
                }
            }
        }

        public void RollBack()
        {
            _logger.LogWarning("Executing Explicit rollback");
            if (IsTransactionFailed)
                throw new InconsistantStateException("Transactional Manager in error state, It would be previously Rollback have been executed. Check logs for more detail");

            IsTransactionFailed = true;
            int TransCount = Transactions.Count - 1;

            for (int index = TransCount; index >= 0; index--)
            {
                ICommandTransaction trac = Transactions[index];
                Dictionary<string, object> Inputs = TransactionInputs[trac];
                string processname = trac.GetType().Name;
                _logger.LogInformation("{0} transaction rollback executing", processname);
                try
                {
                    trac.RollBack(Inputs);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Rollback failed for process {0}", processname);
                    _logger.LogError("Transaction in inconsistant state, manual check required");
                    throw new RollbackFailedException("Rollbacked filed for transaction " + processname, ex);
                }
            }
        }
    }

    public class RollbackFailedException : Exception
    {
        public RollbackFailedException(): base()
        {

        }

        public RollbackFailedException(string msg) : base(msg)
        {

        }

        public RollbackFailedException(string msg, Exception ex): base(msg, ex)
        {

        }
       
        public string TransactionClientName { get; set; }
    }

    public class InconsistantStateException : Exception
    {
        public InconsistantStateException() : base()
        {

        }

        public InconsistantStateException(string msg) : base(msg)
        {

        }

        public InconsistantStateException(string msg, Exception ex) : base(msg, ex)
        {

        }

        public string TransactionClientName { get; set; }
    }

    public class TransactionExecutionException : Exception
    {
        public TransactionExecutionException() : base()
        {

        }

        public TransactionExecutionException(string msg) : base(msg)
        {

        }

        public TransactionExecutionException(string msg, Exception ex) : base(msg, ex)
        {

        }

        public string TransactionClientName { get; set; }
    }
}
