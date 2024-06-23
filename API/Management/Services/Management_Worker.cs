using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Services;
using Business.Management.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Management.Services
{
    // Background Worker: manages many things including watching the appsettings.json for changes 

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




        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            Console.WriteLine("--> Management Service: Background worker is running ...");

            // send requests on startup .....................


            //var result = SendAppsettingsToAllServices();
            var result = HandleAppsettingsUpdate();

        }


        public void OnAppsettingsUpdated(object source, FileSystemEventArgs args)
        {
            if (_switch)
                Console.WriteLine($"--> Appsettings : \n{args.ChangeType}\n{args.Name}\n{args.FullPath}");



            // To Do: PUT requests to all Remote Services with new URLs (but it wouldn't reach all services in case of multiple replicas in K8)


            var result = HandleAppsettingsUpdate();


            _switch = !_switch;
        }



        private IServiceResult<IEnumerable<RemoteService_MODEL_AS>> HandleAppsettingsUpdate()
        {
            IServiceResult<IEnumerable<RemoteService_MODEL_AS>> result;
            var prependMessage = "Management Service:  EVENT -> On Appsettings Update/Change: \n";

            using (var scope = _serviceFactory.CreateScope())
            {
                var _appsettings_Provider = scope.ServiceProvider.GetService<IAppsettings_PROVIDER>();
                var _globalsettings_Provider = scope.ServiceProvider.GetService<IGlobal_Settings_PROVIDER>();


                var appsettingsResult = _appsettings_Provider.GetGlobalConfig();

                appsettingsResult
                    .PrependMessage(" - READ Remote Service models from Global Appsettings. Result: \n")
                    .PrependMessage(prependMessage);

                if (!appsettingsResult.Status)
                    return appsettingsResult;


                var _globalsettingsResult = _globalsettings_Provider.UpdateRemoteServiceModels(appsettingsResult.Data);

                _globalsettingsResult
                    .PrependMessage(" - UPDATE Remote Service models in Global Settings. Result: \n")
                    .PrependMessage(prependMessage);

                if (!_globalsettingsResult.Status)
                    return _globalsettingsResult;


                result = _globalsettings_Provider.GetRemoteServices_WithHTTPClient();

                result
                    .PrependMessage(" - READ updated Remote Service models from Global Settings. Result: \n")
                    .PrependMessage(prependMessage);

                if (!result.Status)
                    return result;
            }




            foreach (var model in result.Data)
            { 
            
                    /////// ================================ To Do: send update to all services ====================
            }




            return result;
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
