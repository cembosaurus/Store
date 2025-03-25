using AutoMapper;
using Business.Inventory.DTOs.ItemPrice;
using Inventory.Models;
using Business.Libraries.ServiceResult;
using Inventory.Services;
using Moq;
using NUnit.Framework;
using Services.Inventory.Data.Repositories.Interfaces;

namespace Store.Test.Unit.Tests.Services.Inventory.Services
{
    [TestFixture]
    internal class ItemPriceService_Test
    {

        private ItemPriceService _itemPriceService;
        private Mock<IItemPriceRepository> _repo = new Mock<IItemPriceRepository>();
        private Mock<IMapper> _mapper = new Mock<IMapper>();

        private int _item1_Id = 1, _item2_Id = 2, _item3_Id = 3;
        private ItemPrice _itemPrice1, _itemPrice2, _itemPrice3, _updatedItemPrice;
        private ItemPriceReadDTO _itemPrice1ReadDTO, _itemPrice2ReadDTO, _itemPrice3ReadDTO, _updatedItemPriceReadDTO;
        private ItemPriceUpdateDTO _itemPriceUpdateDTO;
        private IEnumerable<int> _itemIds;
        private IEnumerable<ItemPrice> _itemPrices_List, _itemPricesIncomplete_List;
        private IEnumerable<ItemPriceReadDTO> _itemPriceReadDTOs_List, _itemPricesIncompleteReadDTO_List;



        [SetUp]
        public void Setup()
        {
            _itemIds = new List<int> { _item1_Id, _item2_Id, _item3_Id };

            _itemPrice1 = new ItemPrice { ItemId = _item1_Id, SalePrice = 10, RRP = 11, DiscountPercent = 1 };
            _itemPrice2 = new ItemPrice { ItemId = _item2_Id, SalePrice = 20, RRP = 21, DiscountPercent = 2 };
            _itemPrice3 = new ItemPrice { ItemId = _item3_Id, SalePrice = 30, RRP = 31, DiscountPercent = 3 };
            _itemPrices_List = new List<ItemPrice> { _itemPrice1, _itemPrice2, _itemPrice3 };
            _itemPricesIncomplete_List = new List<ItemPrice> { _itemPrice1 };

            _itemPrice1ReadDTO = new ItemPriceReadDTO { ItemId = _itemPrice1.ItemId, SalePrice = _itemPrice1.SalePrice, RRP = _itemPrice1.RRP, DiscountPercent = _itemPrice1.DiscountPercent };
            _itemPrice2ReadDTO = new ItemPriceReadDTO { ItemId = _itemPrice2.ItemId, SalePrice = _itemPrice2.SalePrice, RRP = _itemPrice2.RRP, DiscountPercent = _itemPrice2.DiscountPercent };
            _itemPrice3ReadDTO = new ItemPriceReadDTO { ItemId = _itemPrice3.ItemId, SalePrice = _itemPrice3.SalePrice, RRP = _itemPrice3.RRP, DiscountPercent = _itemPrice3.DiscountPercent };
            _itemPriceReadDTOs_List = new List<ItemPriceReadDTO> { _itemPrice1ReadDTO, _itemPrice2ReadDTO, _itemPrice3ReadDTO };
            _itemPricesIncompleteReadDTO_List = new List<ItemPriceReadDTO> { _itemPrice1ReadDTO };

            _itemPriceUpdateDTO = new ItemPriceUpdateDTO { SalePrice = 40, RRP = 41, DiscountPercent = 4 };

            _updatedItemPrice = new ItemPrice
            {
                ItemId = _item1_Id,
                SalePrice = _itemPriceUpdateDTO.SalePrice ?? 0,
                RRP = _itemPriceUpdateDTO.RRP ?? 0,
                DiscountPercent = _itemPriceUpdateDTO.DiscountPercent ?? 0
            };

            _updatedItemPriceReadDTO = new ItemPriceReadDTO
            {
                ItemId = _updatedItemPrice.ItemId,
                SalePrice = _itemPriceUpdateDTO.SalePrice ?? 0,
                RRP = _itemPriceUpdateDTO.RRP ?? 0,
                DiscountPercent = _itemPriceUpdateDTO.DiscountPercent ?? 0
            };


            _itemPriceService = new ItemPriceService(_repo.Object, _mapper.Object, new ServiceResultFactory());
        }




        //  GetItemPrices()

        [Test]
        public void GetItemPrices_WhenCalled_ReturnsItemPricesList()
        {
            _repo.Setup(r => r.GetItemPrices(It.IsAny<IEnumerable<int>>())).Returns(Task.FromResult(_itemPrices_List));

            _mapper.Setup(m => m.Map<IEnumerable<ItemPriceReadDTO>>(_itemPrices_List)).Returns(_itemPriceReadDTOs_List);


            var result = _itemPriceService.GetItemPrices(_itemIds).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_itemIds.Count()));
            Assert.That(result.Data.ElementAt(1).ItemId, Is.EqualTo(_itemIds.ElementAt(1)));
        }


        [Test]
        public void GetItemPrices_SomeItemPricesNotFound_ReturnsItemPricesListAndMessage()
        {
            _repo.Setup(r => r.GetItemPrices(_itemIds)).Returns(Task.FromResult(_itemPricesIncomplete_List));

            _mapper.Setup(m => m.Map<IEnumerable<ItemPriceReadDTO>>(_itemPricesIncomplete_List)).Returns(_itemPricesIncompleteReadDTO_List);


            var result = _itemPriceService.GetItemPrices(_itemIds).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_itemPricesIncomplete_List.Count()));
            Assert.That(result.Data.ElementAt(0).ItemId, Is.EqualTo(_itemIds.ElementAt(0)));
            Assert.That(result.Message, Is.EqualTo($"Prices for {_itemIds.Count() - _itemPricesIncomplete_List.Count()} items were not found ! Reason: Items may not be registered in catalogue."));
        }


        [Test]
        public void GetItemPrices_NoItemPricesFound_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemPrices(It.IsAny<IEnumerable<int>>())).Returns(Task.FromResult(It.IsAny<IEnumerable<ItemPrice>>()));


            var result = _itemPriceService.GetItemPrices(_itemIds).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo("NO item prices found !"));
        }



        // GetItemPriceById()

        [Test]
        public void GetItemPriceById_WhenCalled_ReturnsItemPrice()
        {
            _repo.Setup(r => r.GetItemPriceById(It.IsAny<int>())).Returns(Task.FromResult(_itemPrice1));

            _mapper.Setup(m => m.Map<ItemPriceReadDTO>(_itemPrice1)).Returns(_itemPrice1ReadDTO);


            var result = _itemPriceService.GetItemPriceById(_item1_Id).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_item1_Id));
        }


        [Test]
        public void GetItemPriceById_ItemNotFound_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemPriceById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<ItemPrice>()));

            _repo.Setup(r => r.ItemExistsById(It.IsAny<int>())).Returns(Task.FromResult(false));


            var result = _itemPriceService.GetItemPriceById(_item1_Id).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with Id '{_item1_Id}' NOT found !"));
        }


        [Test]
        public void GetItemPriceById_ItemHasNoPrice_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemPriceById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<ItemPrice>()));

            _repo.Setup(r => r.ItemExistsById(It.IsAny<int>())).Returns(Task.FromResult(true));


            var result = _itemPriceService.GetItemPriceById(_item1_Id).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with Id '{_item1_Id}' NOT found, but Item with Id '{_item1_Id}' EXIST and is NOT labeled with price !"));
        }



        // UpdateItemPrice()

        [Test]
        public void UpdateItemPrice_WhenCalled_UpdatesItemPrice()
        {
            _repo.Setup(r => r.GetItemPriceById(It.IsAny<int>())).Returns(Task.FromResult(_itemPrice1));

            _mapper.Setup(m => m.Map<ItemPrice>(_itemPriceUpdateDTO)).Returns(_updatedItemPrice);

            _mapper.Setup(m => m.Map<ItemPriceReadDTO>(It.IsAny<ItemPrice>())).Returns(_updatedItemPriceReadDTO);

            _repo.Setup(r => r.SaveChanges()).Returns(1);


            var result = _itemPriceService.UpdateItemPrice(_item1_Id, _itemPriceUpdateDTO).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_item1_Id));
            Assert.That(result.Data.SalePrice, Is.EqualTo(_itemPriceUpdateDTO.SalePrice));
            Assert.That(result.Data.RRP, Is.EqualTo(_itemPriceUpdateDTO.RRP));
            Assert.That(result.Data.DiscountPercent, Is.EqualTo(_itemPriceUpdateDTO.DiscountPercent));
        }


        [Test]
        public void UpdateItemPrice_ItemPriceNotFound_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemPriceById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<ItemPrice>()));


            var result = _itemPriceService.UpdateItemPrice(_item1_Id, _itemPriceUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item price '{_item1_Id}': NOT found !"));
        }


        [Test]
        public void UpdateItemPrice_UpdateNotSaved_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemPriceById(It.IsAny<int>())).Returns(Task.FromResult(_itemPrice1));

            _repo.Setup(r => r.SaveChanges()).Returns(0);


            var result = _itemPriceService.UpdateItemPrice(_item1_Id, _itemPriceUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item price '{_item1_Id}': changes were NOT saved into DB !"));
        }


    }
}
