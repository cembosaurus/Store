using AutoMapper;
using Business.Http.Services.Interfaces;
using Business.Management.Appsettings;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;



namespace Management.Services
{
    // Background Worker: events (appsettings changes, ...) 

    public class Management_Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceFactory;
        private FileSystemWatcher _watcher;
        private Config_Global_DB _globalConfig_DB;
        private bool _switch = true; // MS bug - firing event twice. Prevent it by using the switch.
                                     // Not ideal solution as two independet events could be fired in sequence so second one will be ignored.
                                     // But this event is fired rarely f.e: after Appsettings is updated

        public Management_Worker(FileSystemWatcher watcher, Config_Global_DB globalConfig_DB, IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _watcher = watcher;
            _globalConfig_DB = globalConfig_DB;

            Initialize();
        }


        //.................................................................................................. To Do: decopule it, DI appsettings part, .... etc


        // On StartUp:
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            Console.WriteLine("--> Management Service: Background worker is running ...");

            HandleAppsettingsUpdate();
        }


        // On Appsettings Update:
        public void OnAppsettingsUpdated(object source, FileSystemEventArgs args)
        {
            if (_switch)
                Console.WriteLine($"--> Appsettings UPDATE: \n{args.ChangeType}\n{args.Name}\n{args.FullPath}");

            HandleAppsettingsUpdate();


            _switch = !_switch;
        }




        // Send update to all relevant API services (K8 multiple replicas will NOT be reached, only the one selected by Loadbalancer):
        private async void HandleAppsettingsUpdate()
        {
            using (var scope = _serviceFactory.CreateScope())
            {           
                var appsettings_Provider = scope.ServiceProvider.GetService<IAppsettings_PROVIDER>();
                var congigGlobal_Repo = scope.ServiceProvider.GetService<IConfig_Global_REPO>();
                var httpAllServices = scope.ServiceProvider.GetService<IHttpGlobalConfigService>();

                var appsettingsResult = appsettings_Provider.GetGlobalConfig();

                if (appsettingsResult.Status)
                {
                    congigGlobal_Repo.Initialize(appsettingsResult.Data);

                    var httpUpdateResult = await httpAllServices.PostGlobalConfigToMultipleServices();     
                    //.....
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
            _watcher.Filter = "appsettings.Development.json";               
            _watcher.Changed += new FileSystemEventHandler(OnAppsettingsUpdated);
            _watcher.EnableRaisingEvents = true;
        }
    }
}
