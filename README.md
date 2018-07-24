# trasactionmanger
Distribute services transaction manager which execute the web service call in the transaction and rollback if any error occurs

In microservice architecture, there are series of interaction between the micro services. For a simple user action, there could be multiple writes across different services. Failure in one o or multiple service could lead to inconsistent data in system

Idea is to implement transaction across kiwi services which is different from Database transaction and DTC. This design would lead to eventual consistency.

This would be transaction coordinator design which would coordinate the transaction between services and if error occur, would rollback

You could simply download the code and make use of the repository

# Usage
```csharp
KiwiTransactionManager ktm = new KiwiTransactionManager(_logger);


#region Add TransactionClient
ktm.AddTraction(new UpdateStockTransactionalClient(_logger,
"Stock", new Dictionary<string, object> { { "Quantity", 5 } } )
             );
ktm.AddTraction(new CustomerTransactionalClient(_logger, "customer"));
ktm.AddTraction(new VoucherTransactionalClient(_logger, "voucher", new Dictionary<string, object> {  { "Voucher", "abc" }, { "OrderNumber", "KR34342" } })
               );
#endregion

ktm.ExecuteAllInTransaction();
```

# Class Diagram
Transaction Manager Class Diagram
![transaction manager class diagram][ktmclass]

[ktmclass]: https://raw.githubusercontent.com/sbrakl/trasactionmanger/master/images/TransactionManager-ClassDiagram.png