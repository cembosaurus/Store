using Business.Enums;
using Business.Http.Services.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Tools;

namespace Management.Services
{

    public class Management_Worker : BackgroundService
    {

        private readonly IServiceScopeFactory _serviceFactory;
        private FileSystemWatcher _watcher;
        private readonly ConsoleWriter _cm;
        private bool _switch = true; // MS bug - firing event twice. Prevent it by using the switch.
                                     // Not ideal solution as two independet events could be fired in sequence so second one will be ignored.
                                     // But this event is fired rarely f.e: after Appsettings is updated


        public Management_Worker(FileSystemWatcher watcher, IServiceScopeFactory serviceFactory, ConsoleWriter cm)
        {
            _serviceFactory = serviceFactory;
            _watcher = watcher;
            _cm = cm;

            Initialize();
        }


        


        // On StartUp:
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            _cm.Message("App Startup", "Background Worker", "", TypeOfInfo.INFO, "Running...");

            //Thread.Sleep(10000);

            PostGlobalConfigToAPIServices();
        }


        // On Appsettings Update:
        public void OnAppsettingsUpdated(object source, FileSystemEventArgs args)
        {
            if (_switch)
                _cm.Message("Appsettings WRITE event", "Background Worker", "Appsettings was updated...", TypeOfInfo.INFO, $"\n - {args.ChangeType}\n - {args.Name}\n - {args.FullPath}");

            PostGlobalConfigToAPIServices();


            _switch = !_switch;
        }




        // Send update to all relevant API services (K8 multiple replicas will NOT be reached, only the one selected by Loadbalancer):
        private async void PostGlobalConfigToAPIServices()
        {
            _cm.Message("Http Post", "Multiple API Services", "Global Config Update", TypeOfInfo.INFO,"Sending to all relevant API services ...");

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
                        congigGlobal_Repo.Initialize(appsettingsResult.Data ?? null!);

                        var httpUpdateResult = await httpAllServices.PostGlobalConfigToMultipleServices(false);

                        _cm.Message("Http Response", "Multiple API Services", "Global Config Update", TypeOfInfo.INFO,"Sent to API services:");

                        foreach (var service in httpUpdateResult.Data ?? null!)
                        {
                            _cm.Text(
                                ConsoleColor.Black, 
                                ConsoleColor.Cyan, 
                                $" - {service.Key.Name}: ", 
                                ConsoleColor.Black, 
                                service.Key.Name == "ManagementService" ? ConsoleColor.White : service.Value ? ConsoleColor.Yellow : ConsoleColor.Red, 
                                $"{(service.Key.Name == "ManagementService" ? "BYPASSED" : service.Value ? "SUCCESS" : "FAILED")}");
                        }

                        Console.ResetColor();
                    }
                }
                else {
                    _cm.Message("Create Service Scope", "Management Worker", "Unable to send 'Global Config Update' HTTP request to relevant API services!", TypeOfInfo.FAIL, "Some of necessary DI services were not instantiated!");
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
