// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    ApiGatewayUrl: 'http://localhost:2000/',
    IdentityServiceUrl: "http://localhost:2100/",
    InventoryServiceUrl: "http://localhost:2200/",
    OrderingServiceUrl: "http://localhost:2300/",
    PaymentServiceUrl: "http://localhost:2400/",
    SchedulerServiceUrl: "http://localhost:2500/",
    StaticContentUrl: 'http://localhost:2600/'
  };
  
  /*
   * In development mode, to ignore zone related error stack frames such as
   * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
   * import the following file, but please comment it out in production mode
   * because it will have performance impact when throw error
   */
  // import 'zone.js/dist/zone-error';  // Included with Angular CLI.