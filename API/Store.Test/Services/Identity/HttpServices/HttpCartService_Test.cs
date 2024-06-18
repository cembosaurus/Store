using Business.Exceptions.Interfaces;
using Business.Http.Interfaces;
using Business.Inventory.Http.Services;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using System.Net;



namespace Store.Test.Services.Identity.HttpServices
{
    [TestFixture]
    internal class HttpCartService_Test
    {
        private const int _userId = 1;
        private const int _itemId = 5;
        private const int _total = 123;
        private const int _amount = 10;
        private const int _salesPrice = 99;
        private DateTime _locked = DateTime.UtcNow;

        private IHttpCartService _httpCartService;
        private IServiceResultFactory _resultFact;
        private Mock<IHttpAppClient> _httpAppClient;
        private HttpContent _content;

        private IHostingEnvironment env;
        private IExId exId;
        private IAppsettings_PROVIDER appsettingsService;
        private IGlobal_Settings_PROVIDER remoteServices_Provider;


        [SetUp]
        public void Setup()
        {
            _resultFact = new ServiceResultFactory();
            _httpAppClient = new Mock<IHttpAppClient>();

            IServiceResult createCartResult = new ServiceResult<CartReadDTO>(new CartReadDTO
            {
                CartId = new Guid(),
                UserId = _userId,
                Total = _total,
                CartItems = new List<CartItemReadDTO>
                    {
                        new CartItemReadDTO
                        {
                            UserId = _userId,
                            ItemId = _itemId,
                            Name = "Test Item",
                            Amount = _amount,
                            SalePrice = _salesPrice,
                            Locked = _locked
                        }
                    }
            });

            _content = new StringContent(Newtonsoft.Json.JsonConvert
                .SerializeObject(createCartResult));
        }




        [Test]
        public void CreateCart_WhenCalled_ReturnsCartReadDTO()
        {
            //------------------------------------------------------------------------- Temporarely commented out. FIX It later !!!!!!
            //_httpAppClient.Setup(cc => cc.CreateCart(_userId))
            //    .Returns(Task.FromResult( new HttpResponseMessage 
            //    { 
            //        StatusCode = HttpStatusCode.OK, 
            //        Content = _content 
            //    }));


            //_httpCartService = new HttpCartService(_httpAppClient.Object, _resultFact);


            var result = _httpCartService.CreateCart(_userId).Result;


            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.InstanceOf<CartReadDTO>());
        }


        [Test]
        public void CreateCart_WhenFailed_ReturnsNull()
        {
            //------------------------------------------------------------------------- Temporarely commented out. FIX It later !!!!!!
            //_httpAppClient.Setup(cc => cc.CreateCart(_userId))
            //    .Returns(Task.FromResult(new HttpResponseMessage
            //    {
            //        // Anyh other status codes than OK:
            //        StatusCode = HttpStatusCode.InternalServerError,
            //        Content = _content
            //    }));


            //_httpCartService = new HttpCartService(_httpAppClient.Object, _resultFact);


            var result = _httpCartService.CreateCart(_userId).Result;


            Assert.That(result.Data, Is.Null);
        }
    }
}
