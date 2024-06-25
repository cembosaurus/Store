using AutoMapper;
using Business.Http.Services.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Enums;
using Microsoft.IdentityModel.Tokens;



namespace Management.Services
{
    // Background Worker: manages many things including watching the appsettings.json for changes 

    public class Management_Worker : BackgroundService
    {
        private readonly bool _isProdEnv;
        private readonly IServiceScopeFactory _serviceFactory;
        private FileSystemWatcher _watcher;
        private bool _switch = true; // MS bug - firing event twice. Prevent it by using the switch.
                                     // Not ideal solution as two independet events could be fired in sequence so second one will be ignored.
                                     // But this event is fired rarely f.e: after Appsettings is updated


        public Management_Worker(IWebHostEnvironment env, FileSystemWatcher watcher, IServiceScopeFactory serviceFactory)
        {
            _isProdEnv = env.IsProduction();
            _serviceFactory = serviceFactory;
            _watcher = watcher;

            Initialize();
        }





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
                Console.WriteLine($"--> Appsettings : \n{args.ChangeType}\n{args.Name}\n{args.FullPath}");

            HandleAppsettingsUpdate();


            _switch = !_switch;
        }




        // Send update to all relevant API services (K8 multiple replicas will NOT be reached, only the one selected by Loadbalancer):
        private async void HandleAppsettingsUpdate()
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var appsettings_Provider = scope.ServiceProvider.GetService<IAppsettings_PROVIDER>();
                var httpGlobalConfigBroadcast = scope.ServiceProvider.GetService<IHttpGlobalConfigBroadcast>();
                var _mapper = scope.ServiceProvider.GetService<IMapper>();

                var appsettingsResult = appsettings_Provider.GetGlobalConfig();

                if (appsettingsResult.Status)
                {

                    var result = appsettingsResult.Data?.RemoteServices.Where(m => m.GetPathByName(TypeOfService.REST, "GlobalConfig").IsNullOrEmpty() == false).Select(s => s.GetBaseUrl(TypeOfService.REST, _isProdEnv)).ToList();


                    //var httpUpdateResult = await httpGlobalConfigBroadcast.BroadcastUpdate(appsettingsResult.Data);
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
