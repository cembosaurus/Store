using Business.Identity.Http.Services.Interfaces;



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
            Console.WriteLine("TEST");
        }


        public void OnAppsettingsUpdated(object source, FileSystemEventArgs args)
        {
            if (_switch)
                Console.WriteLine($"--> Appsettings : \n{args.ChangeType}\n{args.Name}\n{args.FullPath}");



            // To Do: send Authorized ! PUT requests to all Remote Services with new URLs (but it wouldn't work properly in case of multiple replicas in K8)

            // create scope of IServiceResultFactory inside this singleton:

            using (var scope = _serviceFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IHttpApiKeyAuthService>();

                service.LoginWithApiKey();
            }


            _switch = !_switch;
        }


        // ................................................................ To Do: also appsettings production:
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
