using AutoMapper;
using Business.Identity.DTOs;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Moq;
using NUnit.Framework;
using Ordering.Data.Repositories.Interfaces;
using Ordering.HttpServices.Interfaces;
using Ordering.Services;
using Ordering.Services.Interfaces;
using Services.Ordering.Models;

namespace Store.Test.Services.Ordering.Services
{
    [TestFixture]
    internal class ArchiveService_Test
    {
        private IArchiveService _archiveService;

        private Mock<IArchiveRepository> _archiveRepo = new Mock<IArchiveRepository>();
        private Mock<IHttpAddressService> _httpIdentityService = new Mock<IHttpAddressService>();
        private Mock<IMapper> _mapper = new Mock<IMapper>();
        private IServiceResultFactory _resultFact = new ServiceResultFactory();

        private int _address1Id = 1, _address2Id = 2, _address3Id = 3;
        private IEnumerable<int> _addressIds_List;
        private Order _order1, _order2, _order3;
        private IEnumerable<Order> _orders_List;
        private OrderReadDTO _orderReadDTO1, _orderReadDTO2, _orderReadDTO3;
        private IEnumerable<OrderReadDTO> _orderReadDTOs_List;
        private AddressReadDTO _addressReadDTO1, _addressReadDTO2, _addressReadDTO3;
        private IEnumerable<AddressReadDTO> _addressesReadDTO_List;
        private int _user1Id = 1;
        private Guid _cart1Id = Guid.NewGuid();



        [SetUp]
        public void Setup()
        {
            _addressIds_List = new List<int> { _address1Id, _address2Id, _address3Id};

            _addressReadDTO1 = new AddressReadDTO { AddressId = 1 };
            _addressReadDTO2 = new AddressReadDTO { AddressId = 2 };
            _addressReadDTO3 = new AddressReadDTO { AddressId = 3 };
            _addressesReadDTO_List = new List<AddressReadDTO> { _addressReadDTO1, _addressReadDTO2, _addressReadDTO3};

            _order1 = new Order { OrderDetails = new OrderDetails { AddressId = 1 }, CartId = _cart1Id };
            _order2 = new Order { OrderDetails = new OrderDetails { AddressId = 2 } };
            _order3 = new Order { OrderDetails = new OrderDetails { AddressId = 3 } };
            _orders_List = new List<Order> { _order1, _order2, _order3};

            _orderReadDTO1 = new OrderReadDTO { OrderDetails = new OrderDetailsReadDTO { Address = _addressReadDTO1 }, CartId = _cart1Id };
            _orderReadDTO2 = new OrderReadDTO { OrderDetails = new OrderDetailsReadDTO { Address = _addressReadDTO2 } };
            _orderReadDTO3 = new OrderReadDTO { OrderDetails = new OrderDetailsReadDTO { Address = _addressReadDTO3 } };
            _orderReadDTOs_List = new List<OrderReadDTO> { _orderReadDTO1, _orderReadDTO2, _orderReadDTO3 };

            _archiveService = new ArchiveService(_archiveRepo.Object, _resultFact, _mapper.Object, _httpIdentityService.Object);
        }



        // GetAllOrders()

        [Test]
        public void GetAllOrders_WhenCalled_GetsAllArchivedOrders()
        {
            _archiveRepo.Setup(r => r.GetAllOrders()).Returns(Task.FromResult(_orders_List));

            _httpIdentityService.Setup(i => i.GetAddressesByAddressIds(_addressIds_List))
                .Returns(Task.FromResult(_resultFact.Result(_addressesReadDTO_List, true)));

            _mapper.Setup(m => m.Map<IEnumerable<OrderReadDTO>>(_orders_List)).Returns(_orderReadDTOs_List);


            var result = _archiveService.GetAllOrders().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_orders_List.Count()));
            Assert.That(result.Data.ElementAt(0).OrderDetails.Address.AddressId, Is.EqualTo(_orders_List.ElementAt(0).OrderDetails.AddressId));
        }


        [Test]
        public void GetAllOrders_NoOrdersFound_ReturnsMessage()
        {
            _orders_List = new List<Order>();

            _archiveRepo.Setup(r => r.GetAllOrders()).Returns(Task.FromResult(_orders_List));


            var result = _archiveService.GetAllOrders().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo("NO archived orders were found !"));
        }


        [Test]
        public void GetAllOrders_NoAdressesFound_ReturnsMessage()
        {
            var addressNotFountMessage = "Addresses not found !";

            _archiveRepo.Setup(r => r.GetAllOrders()).Returns(Task.FromResult(_orders_List));

            _httpIdentityService.Setup(i => i.GetAddressesByAddressIds(_addressIds_List))
                .Returns(Task.FromResult(_resultFact.Result<IEnumerable<AddressReadDTO>>(null, false, addressNotFountMessage)));


            var result = _archiveService.GetAllOrders().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"\r\nFailed to obtain addresses for orders from service ! Reason: '{addressNotFountMessage}'"));
        }



        // GetOrderByUserId()

        [Test]
        public void GetOrderByUserId_WhenCalled_ReturnsOrderByUserId()
        {
            _archiveRepo.Setup(ar => ar.GetCartIdByUserId(It.IsAny<int>())).Returns(Task.FromResult(_cart1Id));

            _archiveRepo.Setup(ar => ar.GetOrderByCartId(_cart1Id)).Returns(Task.FromResult(_order1));

            _httpIdentityService.Setup(i => i.GetAddressByAddressId(_address1Id)).Returns(Task.FromResult(_resultFact.Result(_addressReadDTO1, true)));

            _mapper.Setup(m => m.Map<OrderReadDTO>(_order1)).Returns(_orderReadDTO1);


            var result = _archiveService.GetOrderByUserId(_user1Id).Result;
        

            Assert.IsTrue(result.Status);
            Assert.That(result.Data.CartId, Is.EqualTo(_orderReadDTO1.CartId));
        }




    }
}
