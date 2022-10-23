# Persistence Project

The domain project...

## Outbox pattern

This pattern is used to publish a domain events. 

This pattern is useful if we want to be sure that our transaction completes in anatomic way.

Inside the transaction we generate one or more outbox messages and we save them in the outbox. 
Later, we process the outbox and publish the messages one by one, so they are handled by they respective consumers.