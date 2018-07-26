# trasactionmanger
Distribute services transaction manager which execute the web service call in the transaction and rollback if any error occurs

In microservice architecture, there are series of interaction between the micro services. For a simple user action, there could be multiple writes across different services. Failure in one o or multiple service could lead to inconsistent data in system

Idea is to implement transaction across kiwi services which is different from Database transaction and DTC. This design would lead to eventual consistency.

This would be transaction coordinator design which would coordinate the transaction between services and if error occur, would rollback

You could simply clone the repository and walk down the code in **order service**

There are two patterns for the Transactional Manager

### Transaction Manager Based on Builder Pattern
### Transaction Manager Based on Command Pattern

Let Discuss each one of it

# Transaction Manager Based on Builder Pattern
## Usage
https://github.com/sbrakl/trasactionmanger/blob/master/orderservice/Controllers/OrderController.cs
```csharp
BuilderTransactionManager btm = new BuilderTransactionManager(_logger);


#region Add TransactionClient
btm.AddTraction(new UpdateStockTransactionalClient(_logger,
"Stock", new Dictionary<string, object> { { "Quantity", 5 } } )
             );
btm.AddTraction(new CustomerTransactionalClient(_logger, "customer"));
btm.AddTraction(new VoucherTransactionalClient(_logger, "voucher", new Dictionary<string, object> {  { "Voucher", "abc" }, { "OrderNumber", "KR34342" } })
               );
#endregion

btm.ExecuteAllInTransaction();
```

## Class Diagram
Transaction Manager Class Diagram
![transaction manager class diagram][btmclass]


## Where to use
It your transaction are linear and one after the other, this is good usecase to use

# Transaction Manager Based on Command Pattern
## Usage
Here, usage is large, you can look at ProcessOrder2 method to see full implementation
https://github.com/sbrakl/trasactionmanger/blob/master/orderservice/Controllers/OrderController.cs

Below here is just snippet of what happens in ProcessOrder2 method
```csharp
CommandTransactionManager ctm = new CommandTransactionManager(_logger);
try {
       UpdateStockCTransactionClient updateStockclient = new UpdateStockCTransactionClient(_logger);
                updateStockclient.Name = "stock";
                var stockoutput = ctm.ExecuteInTransaction(updateStockclient, 
                                    new Dictionary<string, object> { { "Quantity", 5 } });
                
                //Do some non tractional service call stuff                       
                _logger.LogInformation("Non Tractional Call");
                DummyRepo.UpdateOrder(true) //This would throw RepositoryException
}
catch (TransactionExecutionException isEx)  {
            return "Order processing failed, but rollback were successful";
        }
catch (InconsistantStateException isEx)  {
            return "Order processing failed, code flow error";
        }
catch (RollbackFailedException rlbckEx)  {
            return "Order processing failed, system in incosistant state";
        }
catch (RepositoryException repoEx)  {
            //Since there is exception in the non transactional part
            //I need to manually call RollBack();
            ctm.RollBack();
            return "Order processing failed, but rollback were successful";
        }
```

## Philosophy
If your code flow is complex where there are 
1] Conditional flow where condition design which process become part of transaction
2] You need to handle output of one transaction and give it as input to other transaction
3] There are some non transactional part in between of your transactional flow

Here, CommandTransactionManager gives you freedom to execute transaction as and when required by your code flow. Unlike BuilderTransactionManager transaction, where you need to add all transactions before executing it.

When you execute transaction with CommandTransactionManager, it would add transaction to queue. If any of the subsequent transaction fails, it will call rollback on every transaction which are in the queue in the reverse order of they been added. This is implicit rollback.
If your code flow requires to explicitly call rollback, you can call by calling *Rollback* method. This would rollback all the transaction which till now been added to the transaction manager in the reverse order of they been added. This is explicit rollback.

Check the *ProcessOrder2* method to understand more about the implementation

## Class Diagram
Transaction Manager Class Diagram
![Command manager class diagram][ctmclass]

[btmclass]: https://raw.githubusercontent.com/sbrakl/trasactionmanger/master/images/TransactionManager-ClassDiagram.png

[ctmclass]: https://raw.githubusercontent.com/sbrakl/trasactionmanger/master/images/CommandTransactionManager-ClassDiagram.png