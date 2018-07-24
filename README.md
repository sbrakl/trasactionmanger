# trasactionmanger
Distribute services transaction manager which execute the web service call in the transaction and rollback if any error occurs

In microservice architecture, there are series of interaction between the micro services. For a simple user action, there could be multiple writes across different services. Failure in one o or multiple service could lead to inconsistent data in system

Idea is to implement transaction across kiwi services which is different from Database transaction and DTC. This design would lead to eventual consistency.

This would be transaction coordinator design which would coordinate the transaction between services and if error occur, would rollback

