using Business.Enums;
using Business.Http.Services.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Tools;




namespace Management.Services
{

    public class Management_Worker : BackgroundService
    {

        private readonly IServiceScopeFactory _serviceFactory;
        private FileSystemWatcher _watcher;
        private readonly ConsoleWriter _cm;
        private readonly bool _sendGCToServices;
        private bool _switch; // MS bug - 'FileSystemWatcher.Changed()' is firing event twice for one change! Prevent it by using the switch in 'OnAppsettingsUpdated(...)'.
                              // Not ideal solution as two independet events could be fired in sequence so second one will be ignored.
                              // But this event is fired rarely f.e: after Appsettings is updated



        public Management_Worker(FileSystemWatcher watcher, IServiceScopeFactory serviceFactory, ConsoleWriter cm, bool sendGCToServices)
        {
            _serviceFactory = serviceFactory;
            _watcher = watcher;
            _cm = cm;
            _sendGCToServices = sendGCToServices;

            Initialize();
        }





        // On StartUp:
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cm.Message("App Startup", "Background Worker", "", TypeOfInfo.INFO, "Running...");

            if (!GetAppsettingsIntoGlobalConfig())
                return;

            await PostGlobalConfigToAPIServices();
        }



        // On Appsettings Update:
        public async void OnAppsettingsUpdated(object source, FileSystemEventArgs args)
        {
            if (_switch = !_switch)
                return;

            _cm.Message("Appsettings WRITE event", "Background Worker", "Appsettings was changed", TypeOfInfo.INFO, $"\n - {args.ChangeType}\n - {args.Name}\n - {args.FullPath}");

            if (!GetAppsettingsIntoGlobalConfig())
                return;

            await PostGlobalConfigToAPIServices();

            _switch = default;
        }



        private bool GetAppsettingsIntoGlobalConfig()
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var appsettings_Provider = scope.ServiceProvider.GetService<IAppsettings_PROVIDER>();
                var globalConfig_Provider = scope.ServiceProvider.GetService<IGlobalConfig_PROVIDER>();

                if (appsettings_Provider is not null && globalConfig_Provider is not null)
                {
                    var appsettingsResult = appsettings_Provider.GetGlobalConfig();

                    if (appsettingsResult.Status && appsettingsResult.Data is not null)
                    {
                        globalConfig_Provider.Update(appsettingsResult.Data);

                        return true;
                    }
                    else
                    {
                        _cm.Message("Update local Global Config", "Management Worker", "Unable to copy Appsettings into local Global Config !",
                            TypeOfInfo.WARNING, $"Appsettings was not provided.");
                    }
                }

                _cm.Message("Create Service Scope",
                    "Management Worker",
                    "Unable to send 'Global Config Update' HTTP request to relevant API services!", TypeOfInfo.WARNING, $"Failed to create scope for services: " +
                    $"{(appsettings_Provider is null ? "'HttpAllServices Provider', " : "")}" +
                    $"{(globalConfig_Provider is null ? "'HttpAllServices Provider', " : "")}" +
                    $"");
            }

            return false;
        }



        // Send update to all relevant API services (K8 multiple replicas will NOT be reached, only the one selected by Loadbalancer):
        private async Task PostGlobalConfigToAPIServices()
        {
            if (!_sendGCToServices)
                return;

            _cm.Message("Http Post (outgoing)", "Multiple API Services", "Global Config Update", TypeOfInfo.INFO, "Sending to all relevant API services ...");


            using (var scope = _serviceFactory.CreateScope())
            {

                var httpAllServices = scope.ServiceProvider.GetService<IHttpAllServices>();


                if (httpAllServices is null)
                {
                    _cm.Message("Create Service Scope",
                        "Management Worker",
                        "Unable to send 'Global Config Update' HTTP request to relevant API services!",
                        TypeOfInfo.WARNING, $"Failed to create scope for service: {httpAllServices}");

                    return;
                }


                var httpUpdateResult = await httpAllServices.PostGlobalConfigToMultipleServices(false);

                _cm.Message("HTTP Response (incoming)", "Multiple API Services", "Global Config Update", TypeOfInfo.INFO, "Sent to API services:");

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
