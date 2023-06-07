using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Interfaces;
using Identity.HttpServices;
using Identity.HttpServices.Interfaces;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Text.Json.Nodes;

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
        private Mock<IHttpCartClient> _httpCartClient;
        private HttpContent _content;


        [SetUp]
        public void Setup()
        {
            _resultFact = new ServiceResultFactory();
            _httpCartClient = new Mock<IHttpCartClient>();

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

            _httpCartClient.Setup(cc => cc.CreateCart(_userId))
                .Returns(Task.FromResult( new HttpResponseMessage 
                { 
                    StatusCode = HttpStatusCode.OK, 
                    Content = _content 
                }));


            _httpCartService = new HttpCartService(_httpCartClient.Object, _resultFact);


            var result = _httpCartService.CreateCart(_userId).Result;


            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.InstanceOf<CartReadDTO>());
        }


        [Test]
        public void CreateCart_WhenFailed_ReturnsNull()
        {

            _httpCartClient.Setup(cc => cc.CreateCart(_userId))
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    // Anyh other status codes than OK:
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = _content
                }));


            _httpCartService = new HttpCartService(_httpCartClient.Object, _resultFact);


            var result = _httpCartService.CreateCart(_userId).Result;


            Assert.That(result.Data, Is.Null);
        }
    }
}
