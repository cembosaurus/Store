using Business.Http.Services.Interfaces;
using Business.Management.Appsettings.Interfaces;



namespace Management.Services
{

    public class Management_Worker : BackgroundService
    {

        private readonly IServiceScopeFactory _serviceFactory;
        private FileSystemWatcher _watcher;
        private bool _switch = true; // MS bug - firing event twice. Prevent it by using the switch.
                                     // Not ideal solution as two independet events could be fired in sequence so second one will be ignored.
                                     // But this event is fired rarely f.e: after Appsettings is updated


        public Management_Worker(FileSystemWatcher watcher, IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _watcher = watcher;

            Initialize();
        }


        


        // On StartUp:
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Management Service: Background worker is running.");
            Console.ResetColor();

            PostGlobalConfigToAPIServices();
        }


        // On Appsettings Update:
        public void OnAppsettingsUpdated(object source, FileSystemEventArgs args)
        {
            if (_switch)
                Console.WriteLine($"--> Appsettings UPDATE: \n{args.ChangeType}\n{args.Name}\n{args.FullPath}");

            PostGlobalConfigToAPIServices();


            _switch = !_switch;
        }




        // Send update to all relevant API services (K8 multiple replicas will NOT be reached, only the one selected by Loadbalancer):
        private async void PostGlobalConfigToAPIServices()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("Sending Global Config to all relevant API services ...");

            using (var scope = _serviceFactory.CreateScope())
            {           
                var appsettings_Provider = scope.ServiceProvider.GetService<IAppsettings_PROVIDER>();
                var congigGlobal_Repo = scope.ServiceProvider.GetService<IConfig_Global_REPO>();
                var httpAllServices = scope.ServiceProvider.GetService<IHttpAllServices>();

                if (appsettings_Provider != null
                    && congigGlobal_Repo != null
                    && httpAllServices != null)
                {
                    var appsettingsResult = appsettings_Provider.GetGlobalConfig();

                    if (appsettingsResult.Status)
                    {
                        congigGlobal_Repo.Initialize(appsettingsResult.Data);


                        var httpUpdateResult = await httpAllServices.PostGlobalConfigToMultipleServices(false);


                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n\rGlobal Config was sent to API services:");

                        foreach (var service in httpUpdateResult.Data)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write($" - {service.Key.Name}: ");
                            Console.ForegroundColor = service.Key.Name == "ManagementService" ? ConsoleColor.White : service.Value ? ConsoleColor.Yellow : ConsoleColor.Red;
                            Console.WriteLine($"{(service.Key.Name == "ManagementService" ? "BYPASSED" : service.Value ? "SUCCESS" : "FAILED")}");
                        }

                        Console.ResetColor();
                    }
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAIL: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Unable to send HTTP requests to API services! Some of necessary services were not instantiated!");
                    Console.ResetColor();
                }
            }

        }





        private void Initialize()
        {
            _watcher.Path = Directory.GetCurrentDirectory();
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            //                    | NotifyFilters.Attributes
            //                    | NotifyFilters.CreationTime
            //                    | NotifyFilters.DirectoryName
            //                    | NotifyFilters.FileName
            //                    | NotifyFilters.LastAccess
            //                    | NotifyFilters.Security
            //                    | NotifyFilters.Size;
            _watcher.Filter = "appsettings.json";               
            _watcher.Changed += new FileSystemEventHandler(OnAppsettingsUpdated);
            _watcher.EnableRaisingEvents = true;
        }
    }
}
