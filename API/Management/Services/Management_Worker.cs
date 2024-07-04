using Business.Http.Services.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Newtonsoft.Json;



namespace Management.Services
{

    public class Management_Worker : BackgroundService
    {

        private readonly IServiceScopeFactory _serviceFactory;
        private FileSystemWatcher _watcher;
        private IConfiguration _conf;
        private bool _switch = true; // MS bug - firing event twice. Prevent it by using the switch.
                                     // Not ideal solution as two independet events could be fired in sequence so second one will be ignored.
                                     // But this event is fired rarely f.e: after Appsettings is updated


        public Management_Worker(FileSystemWatcher watcher, IServiceScopeFactory serviceFactory, IConfiguration conf)
        {
            _serviceFactory = serviceFactory;
            _watcher = watcher;
            _conf = conf;

            Initialize();
        }


        


        // On StartUp:
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            Console.WriteLine("--> Management Service: Background worker is running ...");

            HandleAppsettingsUpdate(); /// called just temporarely for testing
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
                var httpAllServices = scope.ServiceProvider.GetService<IHttpAllServices>();


                var managementService = appsettings_Provider.GetRemoteServiceModel("ManagementService");
                var TEST = managementService.Data;
                //string json = System.Text.Json.JsonSerializer.Serialize(managementService.Data);

                var json = JsonConvert.SerializeObject(TEST);//, new JsonSerializerSettings { Formatting = Formatting.Indented });
                var x = System.Text.Json.JsonSerializer.Serialize(TEST);
                var v = JsonConvert.DeserializeObject<object>(json);


                ////ConfigurationFileMap fileMap = new ConfigurationFileMap(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json").ToString()); //Path to your config file
                ////Configuration configuration = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
                //var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //config.AppSettings.Settings["A"].Value = "aaaaaaa";
                //config.Save(ConfigurationSaveMode.Modified);

                //System.Configuration.ConfigurationManager.RefreshSection("appSettings");




                //var write_managementservive = appsettings_Provider.AddOrUpdateAppSetting("A", v);


                //var test = appsettings_Provider.AddOrUpdateAppSetting("A:B:C:Var", "... NEW text ...");

                var TEST_MODEL = _conf.GetSection("Config.Global:RemoteServices").Get<List<RemoteService_AS_MODEL>>().FirstOrDefault(m => m.Name == "StaticContentService");



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
