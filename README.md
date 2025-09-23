In Progress.

To Do: 
- implement distributed logging (API_Gateway is in progress)
- finish error handling
- remove service result return type from classes within apps
- replace non exception error handling by exceptions in placxes that need to shortcut the http requests
- improve the design so the HttpBaseService doesn't need to overload constructor to to prevent circulatory DI: HTTPManagementService <--> GlobalConfig_PROVIDER
- implement Payment and Metrics API services
- upgrade old code in RabbitMQ
- finish the integration tests
