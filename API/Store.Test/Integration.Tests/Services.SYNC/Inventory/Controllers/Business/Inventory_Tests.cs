//using Business.Inventory.DTOs.Item;
//using Business.Libraries.ServiceResult;
//using Business.Libraries.ServiceResult.Interfaces;
//using Business.Management.Appsettings.DTOs;
//using Inventory.Models;
//using Newtonsoft.Json;
//using NUnit.Framework;
//using System.Net;
//using System.Text;




//namespace Tests.Integration.Tests

//{



//    [TestFixture]
//    public class Inventory_Tests : IDisposable
//    {
//        private IServiceResultFactory _resultFact = new ServiceResultFactory();
//        private Inventory_WebApplicationFactory _webAppFactory;
//        private HttpClient _httpClient;
//        private Encoding _encoding = Encoding.UTF8;
//        private string _mediaType = "application/json";
//        private IServiceResult<Config_Global_AS_DTO> _httpManagement_Result;

//        private readonly List<Item> _items_models = new List<Item>
//        {
//            new Item { Id = 1, Description = "Item 1 description", Name = "Item 1", PhotoURL = "Item 1 photo URL" },
//            new Item { Id = 2, Description = "Item 2 description", Name = "Item 2", PhotoURL = "Item 2 photo URL" },
//            new Item { Id = 3, Description = "Item 3 description", Name = "Item 3", PhotoURL = "Item 3 photo URL" },
//            new Item { Id = 4, Description = "Item 4 description", Name = "Item 4", PhotoURL = "Item 4 photo URL" },
//            new Item { Id = 5, Description = "Item 5 description", Name = "Item 5", PhotoURL = "Item 5 photo URL" }
//        };

//        private readonly List<ItemReadDTO> _items_dtos = new List<ItemReadDTO>
//        {
//            new ItemReadDTO { Id = 1, Description = "Item 1 description", Name = "Item 1", PhotoURL = "Item 1 photo URL" },
//            new ItemReadDTO { Id = 2, Description = "Item 2 description", Name = "Item 2", PhotoURL = "Item 2 photo URL" },
//            new ItemReadDTO { Id = 3, Description = "Item 3 description", Name = "Item 3", PhotoURL = "Item 3 photo URL" },
//            new ItemReadDTO { Id = 4, Description = "Item 4 description", Name = "Item 4", PhotoURL = "Item 4 photo URL" },
//            new ItemReadDTO { Id = 5, Description = "Item 5 description", Name = "Item 5", PhotoURL = "Item 5 photo URL" }
//        };



//        [SetUp]
//        public void Setup()
//        {
//            _webAppFactory = new Inventory_WebApplicationFactory();
//            _httpClient = _webAppFactory.CreateClient();
//            _httpManagement_Result = _resultFact.Result(new Config_Global_AS_DTO() { RemoteServices = new List<RemoteService_AS_DTO>() }, true, "");

//            // TEMP solution. To Do: obtain JWT dynamicaly from app.
//            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
//                "Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmFtZSI6IlVzZXJfMyIsInJvbGUiOiJDdXN0b21lciIsIm5iZiI6MTc0MjI2NjIxNSwiZXhwIjoxNzQyODcxMDE1LCJpYXQiOjE3NDIyNjYyMTV9.eFvXQSnTovaNi3QGXjVSXWt-CXgBn_D4aRl33kAIkK0"
//                );
//        }



//        // GetItems


//        [Test]
//        public async Task GetItems_Success_ReturnsListOfItems()
//        {
//            //_webAppFactory.ItemRepo_Mock.Setup(r => r.GetItems(It.IsAny<IEnumerable<int>>())).ReturnsAsync(_items_models);

//            //_webAppFactory.HttpManagementService_Mock.Setup(ms => ms.GetGlobalConfig()).ReturnsAsync(_httpManagement_Result);


//            var response = await _httpClient.GetAsync("/item/all");

//            var result = JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ItemReadDTO>>>(await response.Content.ReadAsStringAsync());


//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            Assert.That(result!.Data!.Count(), Is.EqualTo(_items_dtos.Count));
//            Assert.That(result!.Data!.ElementAt(0).Id, Is.EqualTo(_items_dtos[0].Id));
//            Assert.That(result!.Data!.ElementAt(1).Description, Is.EqualTo(_items_dtos[1].Description));
//            Assert.That(result!.Data!.ElementAt(2).Name, Is.EqualTo(_items_dtos[2].Name));
//            Assert.That(result!.Data!.ElementAt(3).PhotoURL, Is.EqualTo(_items_dtos[3].PhotoURL));
//        }


//        // GetItemById

//        [Test]
//        public async Task GetItemById_Success_ReturnsItem()
//        {
//            var repoResult = new Item { Id = 2, Description = "Item 2 description", Name = "Item 2", PhotoURL = "Item 2 photo URL" };

//            //_webAppFactory.ItemRepo_Mock.Setup(r => r.GetItemById(2)).ReturnsAsync(repoResult);

//            //_webAppFactory.HttpManagementService_Mock.Setup(ms => ms.GetGlobalConfig()).ReturnsAsync(_httpManagement_Result);


//            var response = await _httpClient.GetAsync("/item/2");

//            var appResult = JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(await response.Content.ReadAsStringAsync());


//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            Assert.That(appResult!.Data!.Id, Is.EqualTo(repoResult.Id));
//            Assert.That(appResult!.Data!.Description, Is.EqualTo(repoResult.Description));
//            Assert.That(appResult!.Data!.Name, Is.EqualTo(repoResult.Name));
//        }




//        // AddItem

//        [Test]
//        public async Task AddItel_Success_ReturnsResult()
//        {
//            var itemToAdd = new ItemCreateDTO { Description = "Item description", Name = "new Item 6", PhotoURL = "Item photo URL" };


//            //_webAppFactory.ItemRepo_Mock.Setup(r => r.AddItem(_items_models[2])).ReturnsAsync(EntityState.Added);


//            var response = await _httpClient.PostAsync("/item", new StringContent(JsonConvert.SerializeObject(itemToAdd), _encoding, _mediaType));


//            var appResult = JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(await response.Content.ReadAsStringAsync());


//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            Assert.That(appResult!.Data!.Description, Is.EqualTo(itemToAdd.Description));
//            Assert.That(appResult!.Data!.Name, Is.EqualTo(itemToAdd.Name));
//        }


//        // UpdateItem



//        // DeleteItemById



//        public void Dispose()
//        {
//            _webAppFactory?.Dispose();
//            _httpClient?.Dispose();
//        }



//    }
//}