// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    gatewayUrl: 'http://localhost:5000/',
    IdentityServiceUrl: "http://localhost:6000/",
    InventoryServiceUrl: "http://localhost:7000/",
    OrderingServiceUrl: "http://localhost:8000/",
    PaymentServiceUrl: "http://localhost:10000/",
    SchedulerServiceUrl: "http://localhost:20000/",
    staticContentUrl: 'http://localhost:4000/'
  };
  
  /*
   * In development mode, to ignore zone related error stack frames such as
   * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
   * import the following file, but please comment it out in production mode
   * because it will have performance impact when throw error
   */
  // import 'zone.js/dist/zone-error';  // Included with Angular CLI.