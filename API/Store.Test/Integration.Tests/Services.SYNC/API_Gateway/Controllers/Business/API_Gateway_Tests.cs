using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.DTOs;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Moq;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static Business.Management.Appsettings.Models.RemoteService_AS_MODEL;
using static Business.Management.Appsettings.Models.RemoteService_AS_MODEL.ServiceType;




namespace Tests.Integration.Tests
{


    [TestFixture]
    public class ApiGatewayIntegrationTests
    {

        //private IServiceResultFactory _serviceResultFactory;
        //private Mock<IAppsettings_PROVIDER> _appsettingsProvider_Mock;
        //private Config_Global_AS_MODEL _globalConfig_Sample;
        //private IServiceResult<Config_Global_AS_MODEL> _serviceResult_globalConfig_Sample;

        private WebApplicationFactory<Identity.Program> _identityFactory;
        private WebApplicationFactory<API_Gateway.Program> _gatewayFactory;
        private WebApplicationFactory<Management.Program> _managementFactory;
        private WebApplicationFactory<Inventory.Program> _inventoryFactory;
        private WebApplicationFactory<Metrics.Program> _metricsFactory;

        private HttpClient _identityClient;
        private HttpClient _gatewayClient;
        private HttpClient _managementClient;
        private HttpClient _inventoryClient;
        private HttpClient _metricsClient;



        [SetUp]
        public void Setup()
        {
            //_identityFactory = new WebApplicationFactory<Identity.Program>();

            //_inventoryFactory = new WebApplicationFactory<Inventory.Program>();

            //_managementFactory = new WebApplicationFactory<Management.Program>();

            //_metricsFactory = new WebApplicationFactory<Metrics.Program>();

            //_gatewayFactory = new WebApplicationFactory<API_Gateway.Program>();
                //.WithWebHostBuilder(builder =>
                //{
                //    builder.ConfigureServices(services =>
                //    {
                //        services.Configure<HttpClientFactoryOptions>("ManagementClient", options =>
                //        {
                //            options.HttpClientActions.Add(client =>
                //            {
                //                client.BaseAddress = new Uri(_managementFactory.Server.BaseAddress.ToString());

                //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-api-key", "ApiKey123!");
                //            });
                //        });

                //        services.Configure<HttpClientFactoryOptions>("InventoryClient", options =>
                //        {
                //            options.HttpClientActions.Add(client =>
                //            {
                //                client.BaseAddress = new Uri(_inventoryFactory.Server.BaseAddress.ToString());
                //            });
                //        });

                //        services.Configure<HttpClientFactoryOptions>("MetricsClient", options =>
                //        {
                //            options.HttpClientActions.Add(client =>
                //            {
                //                client.BaseAddress = new Uri(_metricsFactory.Server.BaseAddress.ToString());
                //            });
                //        });
                //    });
                //});


            //_serviceResultFactory = new ServiceResultFactory();

            //_globalConfig_Sample = GetGlobalConfig_Sample();

            //_serviceResult_globalConfig_Sample = _serviceResultFactory.Result(_globalConfig_Sample, true, "");

            //_appsettingsProvider_Mock = new Mock<IAppsettings_PROVIDER>();

            //_appsettingsProvider_Mock.Setup(asp => asp.GetGlobalConfig()).Returns(_serviceResult_globalConfig_Sample);

            //_appsettingsProvider_Mock.Setup(asp => asp.GetRemoteServiceModel(_serviceResult_globalConfig_Sample.Data.RemoteServices[0].Name))
            //    .Returns(_serviceResultFactory.Result(_serviceResult_globalConfig_Sample.Data.RemoteServices.SingleOrDefault(rs => rs.Name == _serviceResult_globalConfig_Sample.Data.RemoteServices[0].Name), true, ""));

            //_appsettingsProvider_Mock.Setup(asp => asp.GetRemoteServiceModel(_serviceResult_globalConfig_Sample.Data.RemoteServices[1].Name))
            //    .Returns(_serviceResultFactory.Result(_serviceResult_globalConfig_Sample.Data.RemoteServices.SingleOrDefault(rs => rs.Name == _serviceResult_globalConfig_Sample.Data.RemoteServices[1].Name), true, ""));

            //_appsettingsProvider_Mock.Setup(asp => asp.GetRemoteServiceModel(_serviceResult_globalConfig_Sample.Data.RemoteServices[2].Name))
            //    .Returns(_serviceResultFactory.Result(_serviceResult_globalConfig_Sample.Data.RemoteServices.SingleOrDefault(rs => rs.Name == _serviceResult_globalConfig_Sample.Data.RemoteServices[2].Name), true, ""));

            //_appsettingsProvider_Mock.Setup(asp => asp.GetRemoteServiceModel(_serviceResult_globalConfig_Sample.Data.RemoteServices[3].Name))
            //    .Returns(_serviceResultFactory.Result(_serviceResult_globalConfig_Sample.Data.RemoteServices.SingleOrDefault(rs => rs.Name == _serviceResult_globalConfig_Sample.Data.RemoteServices[3].Name), true, ""));

        }







        private async Task<string> GetJwtTokenAsync()
        {
            var response = await _identityClient.PostAsync("/Identity/login",
                new StringContent("{ \"Name\": \"User_3\", \"Password\": \"password\" }", Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonResponse);

            var success = doc.RootElement.GetProperty("status").GetBoolean();

            if (!success)
                throw new Exception("Failed to obtain token");


            return doc.RootElement.GetProperty("data").GetString();
        }





        [Test]
        public async Task TestRequest_GetResponseFromAllServices_OK()
        {
            _identityFactory = new WebApplicationFactory<Identity.Program>();
            _inventoryFactory = new WebApplicationFactory<Inventory.Program>();
            _managementFactory = new WebApplicationFactory<Management.Program>();
            //_metricsFactory = new WebApplicationFactory<Metrics.Program>();
            _gatewayFactory = new WebApplicationFactory<API_Gateway.Program>();

            _identityFactory.Server.BaseAddress = new Uri("http://localhost:6001");
            _identityClient = _identityFactory.CreateClient();
            _identityClient.BaseAddress = _identityFactory.Server.BaseAddress;


            // Identity:

            var identityResponse = await _identityClient.PostAsync("/Identity/login",
                new StringContent("{ \"Name\": \"User_3\", \"Password\": \"password\" }", Encoding.UTF8, "application/json"));

            identityResponse.EnsureSuccessStatusCode();

            var identityJson = await identityResponse.Content.ReadAsStringAsync();

            var identityResult = JsonSerializer.Deserialize<ServiceResult<string>>
                (identityJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var token = identityResult?.Data;




            _gatewayFactory.Server.BaseAddress = new Uri("http://localhost:5001");
            _gatewayClient = _gatewayFactory.CreateClient();
            _gatewayClient.BaseAddress = _gatewayFactory.Server.BaseAddress;
            _gatewayClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _managementFactory.Server.BaseAddress = new Uri("http://localhost:30001");
            _managementClient = _managementFactory.CreateClient();
            _managementClient.BaseAddress = _managementFactory.Server.BaseAddress;
            _managementClient.DefaultRequestHeaders.Add("x-api-key", "ApiKey123!");

            _inventoryFactory.Server.BaseAddress = new Uri("http://localhost:7001");
            _inventoryClient = _inventoryFactory.CreateClient();
            _inventoryClient.BaseAddress = _inventoryFactory.Server.BaseAddress;
            _inventoryClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //_metricsFactory.Server.BaseAddress = new Uri("http://localhost:9991");
            //_metricsClient = _metricsFactory.CreateClient();
            //_metricsClient.BaseAddress = _metricsFactory.Server.BaseAddress;
            //_metricsClient.DefaultRequestHeaders.Add("x-api-key", "ApiKey123!");




            // API_Gateway:

            _gatewayClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var gatewayResponse = await _gatewayClient.GetAsync("/item/all");

            gatewayResponse.EnsureSuccessStatusCode();

            var gatewayJson = await gatewayResponse.Content.ReadAsStringAsync();

            var gatewayRersult = JsonSerializer.Deserialize<ServiceResult<IEnumerable<ItemReadDTO>>>
                (gatewayJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            Assert.That(gatewayRersult!.Status, Is.True, "API_Gateway API should return a successful ServiceResult.");



            //// Management:


            //var managementResponse = await _managementClient.GetAsync("/globalconfig");

            //managementResponse.EnsureSuccessStatusCode();

            //var managementJson = await managementResponse.Content.ReadAsStringAsync();

            //var managementRersult = JsonSerializer.Deserialize<ServiceResult<Config_Global_AS_DTO>>
            //    (managementJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            //Assert.That(managementRersult!.Status, Is.True, "Management API should return a successful ServiceResult.");



            //// Inventory:

            //_inventoryClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //var inventoryResponse = await _inventoryClient.GetAsync("/item/all");

            //inventoryResponse.EnsureSuccessStatusCode();

            //var inventoryJson = await inventoryResponse.Content.ReadAsStringAsync();

            //var items = JsonSerializer.Deserialize<ServiceResult<IEnumerable<ItemReadDTO>>>
            //    (inventoryJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            //Assert.That(inventoryResponse.IsSuccessStatusCode, Is.True, "Inventory API should return success.");
        }








        [TearDown]
        public void TearDown()
        {
            _identityClient.Dispose();
            _gatewayClient.Dispose();
            _managementClient.Dispose();
            _inventoryClient.Dispose();
            //_metricsClient.Dispose();

            _identityFactory.Dispose();
            _gatewayFactory.Dispose();
            _managementFactory.Dispose();
            _inventoryFactory.Dispose();
            //_metricsFactory.Dispose();
        }









        private Config_Global_AS_MODEL GetGlobalConfig_Sample()
        {
            return new Config_Global_AS_MODEL
            {
                RemoteServices = new List<RemoteService_AS_MODEL> {
                                new RemoteService_AS_MODEL { Name = "ManagementService"
                                , Type = new List<ServiceType> {
                                    new ServiceType {
                                        Name = "REST",
                                        BaseURL = new SchemeHostPort { Dev = "http://localhost", Prod = "" },
                                        Paths = new List<URLPath>{
                                            new URLPath { Name = "GlobalConfig", Route = "globalconfig" }
                                        }
                                    }
                                }},
                                new RemoteService_AS_MODEL { Name = "InventoryService"
                                , Type = new List<ServiceType> {
                                    new ServiceType {
                                        Name = "REST",
                                        BaseURL = new SchemeHostPort { Dev = "http://localhost", Prod = "" },
                                        Paths = new List<URLPath>{
                                            new URLPath { Name = "GlobalConfig", Route = "globalconfig" },
                                            new URLPath { Name = "Item", Route = "item" }
                                        }
                                    }
                                }},
                                new RemoteService_AS_MODEL { Name = "IdentityService"
                                , Type = new List<ServiceType> {
                                    new ServiceType {
                                        Name = "REST",
                                        BaseURL = new SchemeHostPort { Dev = "http://localhost", Prod = "" },
                                        Paths = new List<URLPath>{
                                            new URLPath { Name = "GlobalConfig", Route = "globalconfig" },
                                            new URLPath { Name = "Identity", Route = "identity" }
                                        }
                                    }
                                }},
                                new RemoteService_AS_MODEL { Name = "MetricsService"
                                , Type = new List<ServiceType> {
                                    new ServiceType {
                                        Name = "REST",
                                        BaseURL = new SchemeHostPort { Dev = "http://localhost", Prod = "" },
                                        Paths = new List<URLPath>{
                                            new URLPath { Name = "GlobalConfig", Route = "globalconfig" },
                                            new URLPath { Name = "Collector", Route = "collector" }
                                        }
                                    }
                                }}
                            },
                Auth = new Auth_AS_MODEL
                {
                    JWTKey = "I know it's only rock 'n' roll but I like it",
                    ApiKey = "ApiKey123!"
                }

            };
        }
    }










    //public class API_Gateway_Tests
    //{


    //    private IServiceResultFactory _resultFact = new ServiceResultFactory();
    //    private API_Gateway_WebApplicationFactory _webAppFactory;
    //    private HttpClient _httpClient;
    //    private Encoding _encoding = Encoding.UTF8;
    //    private string _mediaType = "application/json";
    //    private IServiceResult<Config_Global_AS_DTO> _httpManagement_Result;

    //    private readonly List<Item> _items_models = new List<Item>
    //    {
    //        new Item { Id = 1, Description = "Item 1 description", Name = "Item 1", PhotoURL = "Item 1 photo URL" },
    //        new Item { Id = 2, Description = "Item 2 description", Name = "Item 2", PhotoURL = "Item 2 photo URL" },
    //        new Item { Id = 3, Description = "Item 3 description", Name = "Item 3", PhotoURL = "Item 3 photo URL" },
    //        new Item { Id = 4, Description = "Item 4 description", Name = "Item 4", PhotoURL = "Item 4 photo URL" },
    //        new Item { Id = 5, Description = "Item 5 description", Name = "Item 5", PhotoURL = "Item 5 photo URL" }
    //    };

    //    private readonly List<ItemReadDTO> _items_dtos = new List<ItemReadDTO>
    //    {
    //        new ItemReadDTO { Id = 1, Description = "Item 1 description", Name = "Item 1", PhotoURL = "Item 1 photo URL" },
    //        new ItemReadDTO { Id = 2, Description = "Item 2 description", Name = "Item 2", PhotoURL = "Item 2 photo URL" },
    //        new ItemReadDTO { Id = 3, Description = "Item 3 description", Name = "Item 3", PhotoURL = "Item 3 photo URL" },
    //        new ItemReadDTO { Id = 4, Description = "Item 4 description", Name = "Item 4", PhotoURL = "Item 4 photo URL" },
    //        new ItemReadDTO { Id = 5, Description = "Item 5 description", Name = "Item 5", PhotoURL = "Item 5 photo URL" }
    //    };



    //    [SetUp]
    //    public void Setup()
    //    {
    //        _webAppFactory = new API_Gateway_WebApplicationFactory();
    //        _httpClient = _webAppFactory.CreateClient();
    //        _httpManagement_Result = _resultFact.Result(new Config_Global_AS_DTO() { RemoteServices = new List<RemoteService_AS_DTO>() }, true, "");

    //        // TEMP solution. To Do: obtain JWT dynamicaly from app.
    //        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
    //            "Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmFtZSI6IlVzZXJfMyIsInJvbGUiOiJDdXN0b21lciIsIm5iZiI6MTc0MjI2NjIxNSwiZXhwIjoxNzQyODcxMDE1LCJpYXQiOjE3NDIyNjYyMTV9.eFvXQSnTovaNi3QGXjVSXWt-CXgBn_D4aRl33kAIkK0"
    //            );
    //    }





    //    // --- Inventory:




    //    // GetItems:


    //    [Test]
    //    public async Task GetItems_Success_ReturnsListOfItems()
    //    {

    //        var response = await _httpClient.GetAsync("/item/all");

    //        var result = JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ItemReadDTO>>>(await response.Content.ReadAsStringAsync());


    //        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    //        Assert.That(result!.Data!.Count(), Is.EqualTo(_items_dtos.Count));
    //        Assert.That(result!.Data!.ElementAt(0).Id, Is.EqualTo(_items_dtos[0].Id));
    //        Assert.That(result!.Data!.ElementAt(1).Description, Is.EqualTo(_items_dtos[1].Description));
    //        Assert.That(result!.Data!.ElementAt(2).Name, Is.EqualTo(_items_dtos[2].Name));
    //        Assert.That(result!.Data!.ElementAt(3).PhotoURL, Is.EqualTo(_items_dtos[3].PhotoURL));
    //    }





}

