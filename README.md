Services:

- API Gateway
- Identity
- Inventory
- Inventory (RabbitMQ)
- Management
- Metrics
- Ordering
- Payment
- Scheduler
- Static Content


Kubernetes:
  
  - services deployemnts
  - Ingress
  - Logging
  - Secrets
  - Cluster IP
  - Node Ports (for dev)
  - Horiozntal Scaling
  - Load Balancing
  - DBs
  - Volumes


*****************************************************************************************************************************************************************

In Progress.

To Do: 
- implement distributed logging (API_Gateway is in progress)
- finish error handling
- inspect DI scopes - replace scoped by stateless singletons if possible (to improve performance)
- replace temporary 'anyone' authorization policy in controllers used for dev by actual ones
- remove service result return type from methods
- replace non exception error handling by exceptions in placxes that need to shortcut the http requests
- improve the design so the HttpBaseService doesn't need to overload constructor to to prevent circulatory DI: HTTPManagementService <--> GlobalConfig_PROVIDER
- implement Payment and Metrics API services
- upgrade old code in RabbitMQ
- finish the integration tests
- paging
