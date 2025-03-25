using AutoMapper;
using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.DTOs.Item;
using Business.Inventory.DTOs.ItemPrice;
using Inventory.Models;
using Business.Libraries.ServiceResult;
using Inventory.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Services.Inventory.Data.Repositories.Interfaces;

namespace Store.Test.Unit.Tests.Services.Inventory.Services
{
    [TestFixture]
    public class CatalogueItemService_Test
    {
        private CatalogueItemService _catalogueItemService;

        private Mock<ICatalogueItemRepository> _catalogueItemRepo = new Mock<ICatalogueItemRepository>();
        private Mock<IItemRepository> _itemRepo = new Mock<IItemRepository>();
        private Mock<IMapper> _mapper = new Mock<IMapper>();

        private int _item1_Id = 1, _item2_Id = 2, _item3_Id = 3, _item4_Id = 4, _item5_Id = 5;
        private Item _item1, _item2, _item3, _item4, _item5;
        private List<Item> _items;
        private ItemReadDTO _item1ReadDTO, _item4ReadDTO, _item5ReadDTO;
        private CatalogueItem _catalogueItem1, _catalogueItem2, _catalogueItem3, _catalogueItem1WithExtras, _newCatalogueItem, _updatedCatalogueItem;
        private List<CatalogueItem> _catalogueItems, _accessories, _similarProducts;
        private CatalogueItemReadDTO _catalogueItem1ReadDTO, _catalogueItem2ReadDTO, _catalogueItem3ReadDTO, _newCatalogueItemReadDTO, _updatedCatalogueItemReadDTO;
        private CatalogueItemCreateDTO _catalogueItemCreateDTO;
        private CatalogueItemUpdateDTO _catalogueItemUpdateDTO;
        private CatalogueItemReadDTOWithExtras _catalogueItemReadDTOWithExtras;
        private ExtrasAddDTO _extrasAddDTO;
        private List<CatalogueItemReadDTO> _catalogueItemReadDTO_List, _accessoriesReadDTO_List, _similarProductsReadDTO_List;
        private AccessoryItem _accessoryItem1;
        private SimilarProductItem _similarProductItem1;
        private List<AccessoryItem> _accessoryItems;
        private List<SimilarProductItem> _similarProductItems;



        [SetUp]
        public void Setup()
        {
            _item1 = new Item { Id = _item1_Id, Name = "Item 1", Description = "Description of Item 1", PhotoURL = "photo url of Item 1", Archived = false };
            _item2 = new Item { Id = _item2_Id, Name = "Item 2", Description = "Description of Item 2", PhotoURL = "photo url of Item 2", Archived = true };
            _item3 = new Item { Id = _item3_Id, Name = "Item 3", Description = "Description of Item 3", PhotoURL = "photo url of Item 3", Archived = true };
            _items = new List<Item> { _item1, _item2, _item3 };
            _item4 = new Item { Id = _item4_Id, Name = "Item 4", Description = "Description of Item 4", PhotoURL = "photo url of Item 4", Archived = true };
            _item5 = new Item { Id = _item5_Id, Name = "Item 5", Description = "Description of Item 5", PhotoURL = "photo url of Item 5", Archived = true };

            _item1ReadDTO = new ItemReadDTO { Id = _item1.Id, Description = _item1.Description, Name = _item1.Name, PhotoURL = _item1.PhotoURL };

            _catalogueItem1 = new CatalogueItem { ItemId = _item1.Id, Description = "Description of catalogue item 1", Instock = 11, Item = _item1 };
            _catalogueItem2 = new CatalogueItem { ItemId = _item2.Id, Description = "Description of catalogue item 2", Instock = 22, Item = _item2 };
            _catalogueItem3 = new CatalogueItem { ItemId = _item3.Id, Description = "Description of catalogue item 3", Instock = 33, Item = _item3 };

            _accessories = new List<CatalogueItem> { _catalogueItem2 };
            _similarProducts = new List<CatalogueItem> { _catalogueItem3 };
            _accessoryItem1 = new AccessoryItem { ItemId = _item1.Id, AccessoryItemId = _item2.Id };
            _similarProductItem1 = new SimilarProductItem { ItemId = _item1.Id, SimilarProductItemId = _item3.Id };

            _accessoryItems = new List<AccessoryItem> { new AccessoryItem { ItemId = _item1.Id, AccessoryItemId = _accessoryItem1.AccessoryItemId } };
            _similarProductItems = new List<SimilarProductItem> { new SimilarProductItem { ItemId = _item1.Id, SimilarProductItemId = _similarProductItem1.SimilarProductItemId } };

            _catalogueItem1WithExtras = new CatalogueItem
            {
                ItemId = _catalogueItem1.ItemId,
                Description = _catalogueItem1.Description,
                Instock = _catalogueItem1.Instock,
                Item = _catalogueItem1.Item,
                Accessories = _accessoryItems,
                SimilarProducts = _similarProductItems
            };
            _catalogueItems = new List<CatalogueItem> { _catalogueItem1, _catalogueItem2, _catalogueItem3 };


            _catalogueItem1ReadDTO = new CatalogueItemReadDTO { ItemId = _catalogueItem1.ItemId, Description = _catalogueItem1.Description, Instock = _catalogueItem1.Instock };
            _catalogueItem2ReadDTO = new CatalogueItemReadDTO { ItemId = _catalogueItem2.ItemId, Description = _catalogueItem2.Description, Instock = _catalogueItem2.Instock };
            _catalogueItem3ReadDTO = new CatalogueItemReadDTO { ItemId = _catalogueItem3.ItemId, Description = _catalogueItem3.Description, Instock = _catalogueItem3.Instock };
            _catalogueItemReadDTOWithExtras = new CatalogueItemReadDTOWithExtras
            {
                ItemId = _item1.Id,
                Description = _catalogueItem1.Description,
                Item = _item1ReadDTO,
                ItemPrice = new ItemPriceReadDTO
                {
                    ItemId = _item1.Id,
                    DiscountPercent = 10,
                    RRP = 100,
                    SalePrice = 200
                },
                Instock = 50,
                AccessoryItems = _accessoriesReadDTO_List,
                SimilarProductItems = _similarProductsReadDTO_List
            };
            _catalogueItemReadDTO_List = new List<CatalogueItemReadDTO> { _catalogueItem1ReadDTO, _catalogueItem2ReadDTO, _catalogueItem3ReadDTO };

            _accessoriesReadDTO_List = new List<CatalogueItemReadDTO> { _catalogueItem2ReadDTO };
            _similarProductsReadDTO_List = new List<CatalogueItemReadDTO> { _catalogueItem3ReadDTO };

            _catalogueItemCreateDTO = new CatalogueItemCreateDTO
            {
                Description = "NEW Catalogue Item 4 description",
                ItemPrice = new ItemPriceCreateDTO
                {
                    SalePrice = 4000,
                    RRP = 4001,
                    DiscountPercent = 40
                },
                Instock = 400
            };

            _catalogueItemUpdateDTO = new CatalogueItemUpdateDTO
            {
                Description = "UPDATED Catalogue Item 4 description",
                ItemPrice = new ItemPriceUpdateDTO
                {
                    SalePrice = 5000,
                    RRP = 5001,
                    DiscountPercent = 50
                },
                Instock = 500
            };

            _newCatalogueItem = new CatalogueItem
            {
                ItemId = _item4.Id,
                Item = _item4,
                Description = _catalogueItemCreateDTO.Description,
                Instock = _catalogueItemCreateDTO.Instock ?? 0,
                ItemPrice = new ItemPrice
                {
                    SalePrice = _catalogueItemCreateDTO.ItemPrice.SalePrice,
                    RRP = _catalogueItemCreateDTO.ItemPrice.RRP,
                    DiscountPercent = _catalogueItemCreateDTO.ItemPrice.DiscountPercent
                }
            };

            _updatedCatalogueItem = new CatalogueItem
            {
                ItemId = _item5.Id,
                Item = _item5,
                Description = _catalogueItemUpdateDTO.Description,
                Instock = _catalogueItemUpdateDTO.Instock ?? 0,
                ItemPrice = new ItemPrice
                {
                    SalePrice = _catalogueItemUpdateDTO.ItemPrice.SalePrice ?? 5000,
                    RRP = _catalogueItemUpdateDTO.ItemPrice.RRP,
                    DiscountPercent = _catalogueItemUpdateDTO.ItemPrice.DiscountPercent
                }
            };

            _item4ReadDTO = new ItemReadDTO { Id = _item4.Id, Description = _item4.Description, Name = _item4.Name, PhotoURL = _item4.PhotoURL };
            _item5ReadDTO = new ItemReadDTO { Id = _item5.Id, Description = _item5.Description, Name = _item5.Name, PhotoURL = _item5.PhotoURL };

            _newCatalogueItemReadDTO = new CatalogueItemReadDTO
            {
                ItemId = _newCatalogueItem.ItemId,
                Description = _newCatalogueItem.Description,
                Instock = _newCatalogueItem.Instock,
                Item = _item4ReadDTO,
                ItemPrice = new ItemPriceReadDTO
                {
                    SalePrice = _newCatalogueItem.ItemPrice.SalePrice,
                    RRP = _newCatalogueItem.ItemPrice.RRP,
                    DiscountPercent = _newCatalogueItem.ItemPrice.DiscountPercent
                }
            };

            _updatedCatalogueItemReadDTO = new CatalogueItemReadDTO
            {
                ItemId = _updatedCatalogueItem.ItemId,
                Description = _updatedCatalogueItem.Description,
                Instock = _updatedCatalogueItem.Instock,
                Item = _item5ReadDTO,
                ItemPrice = new ItemPriceReadDTO
                {
                    SalePrice = _updatedCatalogueItem.ItemPrice.SalePrice,
                    RRP = _updatedCatalogueItem.ItemPrice.RRP,
                    DiscountPercent = _updatedCatalogueItem.ItemPrice.DiscountPercent
                }
            };

            _extrasAddDTO = new ExtrasAddDTO
            {
                AccessoryIds = new List<int> { 2, 3 },
                SimilarProductIds = new List<int> { 4 }
            };

            _catalogueItemService = new CatalogueItemService(_catalogueItemRepo.Object, _itemRepo.Object, _mapper.Object, new ServiceResultFactory());
        }




        //  GetCatalogueItems()

        [Test]
        public void GetCatalogueItems_WhenCalled_ReturnsListOfCatalogueItems()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItems(It.IsAny<IEnumerable<int>>())).Returns(Task.FromResult(_catalogueItems.AsEnumerable()));

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_catalogueItems)).Returns(_catalogueItemReadDTO_List);


            var result = _catalogueItemService.GetCatalogueItems(It.IsAny<IEnumerable<int>>()).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_catalogueItems.Count()));
            Assert.That(result.Data.ElementAt(0).ItemId, Is.EqualTo(_catalogueItems.ElementAt(0).ItemId));
        }


        [Test]
        public void GetCatalogueItems_CatalogueItemsNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItems(It.IsAny<IEnumerable<int>>())).Returns(Task.FromResult(new List<CatalogueItem>().AsEnumerable()));

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_catalogueItems)).Returns(It.IsAny<IEnumerable<CatalogueItemReadDTO>>());


            var result = _catalogueItemService.GetCatalogueItems(It.IsAny<IEnumerable<int>>()).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo("NO catalogue items found !"));
        }


        //  GetCatalogueItemById()

        [Test]
        public void GetCatalogueItemById_WhenCalled_ReturnsCatalogueItem()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _mapper.Setup(m => m.Map<CatalogueItemReadDTO>(_catalogueItem1)).Returns(_catalogueItem1ReadDTO);


            var result = _catalogueItemService.GetCatalogueItemById(It.IsAny<int>()).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_catalogueItem1.ItemId));
        }


        [Test]
        public void GetCatalogueItemById_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(_catalogueItem1.ItemId)).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));

            _itemRepo.Setup(r => r.ExistsById(_catalogueItem1.ItemId)).Returns(Task.FromResult(false));

            _mapper.Setup(m => m.Map<CatalogueItemReadDTO>(_catalogueItem1)).Returns(It.IsAny<CatalogueItemReadDTO>());


            var result = _catalogueItemService.GetCatalogueItemById(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue Item with id '{_catalogueItem1.ItemId}' NOT found"));
        }


        [Test]
        public void GetCatalogueItemById_CatalogueItemNotFoundButRelatedeItemExists_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(_catalogueItem1.ItemId)).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));

            _itemRepo.Setup(r => r.ExistsById(_catalogueItem1.ItemId)).Returns(Task.FromResult(true));

            _mapper.Setup(m => m.Map<CatalogueItemReadDTO>(_catalogueItem1)).Returns(It.IsAny<CatalogueItemReadDTO>());


            var result = _catalogueItemService.GetCatalogueItemById(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue Item with id '{_catalogueItem1.ItemId}' NOT found\r\n, but Item with id '{_item1_Id}' exists and it's not registered in catalogue !"));
        }



        //  ExistsCatalogueItemById()

        [Test]
        public void ExistsCatalogueItemById_WhenCalled_ReturnsBoolean()
        {
            _catalogueItemRepo.Setup(r => r.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(true));


            var result = _catalogueItemService.ExistsCatalogueItemById(It.IsAny<int>()).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.True);
        }


        [Test]
        public void ExistsCatalogueItemById_ItemDoesNotExist_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.ExistsById(_catalogueItem1.ItemId)).Returns(Task.FromResult(false));


            var result = _catalogueItemService.ExistsCatalogueItemById(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.False);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item '{_catalogueItem1.ItemId}' does NOT exist !"));
        }


        //  GetCatalogueItemWithExtrasById()

        [Test]
        public void GetCatalogueItemWithExtrasById_WhenCalled_ReturnsCatalogueItemWithExtras()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId)).Returns(Task.FromResult(_catalogueItem1WithExtras));

            _catalogueItemRepo.Setup(r => r.GetAccessoriesForCatalogueItem(It.IsAny<CatalogueItem>())).Returns(Task.FromResult(_accessories.AsEnumerable()));

            _catalogueItemRepo.Setup(r => r.GetSimilarProductsForCatalogueItem(It.IsAny<CatalogueItem>())).Returns(Task.FromResult(_similarProducts.AsEnumerable()));

            _mapper.Setup(m => m.Map<CatalogueItemReadDTOWithExtras>(It.IsAny<CatalogueItem>())).Returns(_catalogueItemReadDTOWithExtras);

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_accessories)).Returns(_accessoriesReadDTO_List);

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_similarProducts)).Returns(_similarProductsReadDTO_List);


            var result = _catalogueItemService.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_catalogueItem1.ItemId));
            Assert.That(result.Data.AccessoryItems.ElementAt(0).ItemId, Is.EqualTo(_accessories.ElementAt(0).ItemId));
            Assert.That(result.Data.SimilarProductItems.ElementAt(0).ItemId, Is.EqualTo(_similarProducts.ElementAt(0).ItemId));
        }


        [Test]
        public void GetCatalogueItemWithExtrasById_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId)).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));


            var result = _catalogueItemService.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with item id '{_catalogueItem1.ItemId}' NOT found !"));

        }


        [Test]
        public void GetCatalogueItemWithExtrasById_AccessoriesNotFound_ReturnsCatalogueItemWithMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId)).Returns(Task.FromResult(_catalogueItem1WithExtras));

            _catalogueItemRepo.Setup(r => r.GetAccessoriesForCatalogueItem(It.IsAny<CatalogueItem>())).Returns(Task.FromResult(It.IsAny<IEnumerable<CatalogueItem>>()));

            _catalogueItemRepo.Setup(r => r.GetSimilarProductsForCatalogueItem(It.IsAny<CatalogueItem>())).Returns(Task.FromResult(_similarProducts.AsEnumerable()));

            _catalogueItemReadDTOWithExtras.AccessoryItems = null;

            _mapper.Setup(m => m.Map<CatalogueItemReadDTOWithExtras>(It.IsAny<CatalogueItem>())).Returns(_catalogueItemReadDTOWithExtras);

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_accessories)).Returns(_accessoriesReadDTO_List);

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_similarProducts)).Returns(_similarProductsReadDTO_List);


            var result = _catalogueItemService.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_catalogueItem1.ItemId));
            Assert.That(result.Data.AccessoryItems, Is.Null);
            Assert.That(result.Data.SimilarProductItems.ElementAt(0).ItemId, Is.EqualTo(_similarProducts.ElementAt(0).ItemId));
            Assert.That(result.Message, Is.EqualTo($"\r\nAccessoriesw for item '{_catalogueItem1.ItemId}' not found."));
        }


        [Test]
        public void GetCatalogueItemWithExtrasById_SimilarItemsNotFound_ReturnsCatalogueItemWithMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId)).Returns(Task.FromResult(_catalogueItem1WithExtras));

            _catalogueItemRepo.Setup(r => r.GetAccessoriesForCatalogueItem(It.IsAny<CatalogueItem>())).Returns(Task.FromResult(_accessories.AsEnumerable()));

            _catalogueItemRepo.Setup(r => r.GetSimilarProductsForCatalogueItem(It.IsAny<CatalogueItem>())).Returns(Task.FromResult(It.IsAny<IEnumerable<CatalogueItem>>()));

            _catalogueItemReadDTOWithExtras.SimilarProductItems = null;

            _mapper.Setup(m => m.Map<CatalogueItemReadDTOWithExtras>(It.IsAny<CatalogueItem>())).Returns(_catalogueItemReadDTOWithExtras);

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_accessories)).Returns(_accessoriesReadDTO_List);

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_similarProducts)).Returns(_similarProductsReadDTO_List);


            var result = _catalogueItemService.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_catalogueItem1.ItemId));
            Assert.That(result.Data.AccessoryItems.ElementAt(0).ItemId, Is.EqualTo(_accessories.ElementAt(0).ItemId));
            Assert.That(result.Data.SimilarProductItems, Is.Null);
            Assert.That(result.Message, Is.EqualTo($"\r\nSimilar products for item '{_catalogueItem1.ItemId}' not found."));
        }


        [Test]
        public void GetCatalogueItemWithExtrasById_AccessoriesAndSimilarItemsNotFound_ReturnsCatalogueItemWithMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId)).Returns(Task.FromResult(_catalogueItem1WithExtras));

            _catalogueItemRepo.Setup(r => r.GetAccessoriesForCatalogueItem(It.IsAny<CatalogueItem>()))
                .Returns(Task.FromResult(It.IsAny<IEnumerable<CatalogueItem>>()));

            _catalogueItemRepo.Setup(r => r.GetSimilarProductsForCatalogueItem(It.IsAny<CatalogueItem>()))
                .Returns(Task.FromResult(It.IsAny<IEnumerable<CatalogueItem>>()));

            _catalogueItemReadDTOWithExtras.AccessoryItems = null;
            _catalogueItemReadDTOWithExtras.SimilarProductItems = null;

            _mapper.Setup(m => m.Map<CatalogueItemReadDTOWithExtras>(It.IsAny<CatalogueItem>())).Returns(_catalogueItemReadDTOWithExtras);

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_accessories)).Returns(_accessoriesReadDTO_List);

            _mapper.Setup(m => m.Map<IEnumerable<CatalogueItemReadDTO>>(_similarProducts)).Returns(_similarProductsReadDTO_List);


            var result = _catalogueItemService.GetCatalogueItemWithExtrasById(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_catalogueItem1.ItemId));
            Assert.That(result.Data.AccessoryItems, Is.Null);
            Assert.That(result.Data.SimilarProductItems, Is.Null);
            Assert.That(result.Message, Is.EqualTo($"\r\nAccessoriesw for item '{_catalogueItem1.ItemId}' not found." +
                $"\r\nSimilar products for item '{_catalogueItem1.ItemId}' not found."));
        }



        //  CreateCatalogueItem()

        [Test]
        public void CreateCatalogueItem_WhenCalled_CreatesCatalogueItemInRepo()
        {
            _catalogueItemRepo.Setup(r => r.ExistsById(_catalogueItem1.ItemId)).Returns(Task.FromResult(false));

            _itemRepo.Setup(r => r.GetItemById(It.IsAny<int>())).Returns(Task.FromResult(_item4));

            _catalogueItemRepo.Setup(r => r.CreateCatalogueItem(It.IsAny<CatalogueItem>())).Returns(Task.FromResult(EntityState.Added));
            // OR:
            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(1);

            _mapper.Setup(m => m.Map<CatalogueItem>(It.IsAny<Item>())).Returns(_newCatalogueItem);

            _mapper.Setup(m => m.Map<CatalogueItem>(It.IsAny<CatalogueItemCreateDTO>())).Returns(_newCatalogueItem);

            _mapper.Setup(m => m.Map<CatalogueItemReadDTO>(It.IsAny<CatalogueItem>())).Returns(_newCatalogueItemReadDTO);


            var result = _catalogueItemService.CreateCatalogueItem(_item1.Id, _catalogueItemCreateDTO).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_item4.Id));
            Assert.That(result.Data.ItemPrice.SalePrice, Is.EqualTo(_catalogueItemCreateDTO.ItemPrice.SalePrice));
            Assert.That(result.Data.Description, Is.EqualTo(_catalogueItemCreateDTO.Description));
            _catalogueItemRepo.Verify(r => r.SaveChanges());
        }


        [Test]
        public void CreateCatalogueItem_CatalogueItemAlreadyExists_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.ExistsById(_catalogueItem1.ItemId)).Returns(Task.FromResult(true));


            var result = _catalogueItemService.CreateCatalogueItem(_item1.Id, _catalogueItemCreateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with id '{_catalogueItem1.ItemId}' already EXISTS !"));
        }


        [Test]
        public void CreateCatalogueItem_ItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.ExistsById(_catalogueItem1.ItemId)).Returns(Task.FromResult(false));

            _itemRepo.Setup(r => r.GetItemById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<Item>()));


            var result = _catalogueItemService.CreateCatalogueItem(_item1.Id, _catalogueItemCreateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item with id '{_catalogueItem1.ItemId}' NOT found !"));
        }


        [Test]
        public void CreateCatalogueItem_CatalogueItemNotCreated_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.ExistsById(_catalogueItem1.ItemId)).Returns(Task.FromResult(false));

            _itemRepo.Setup(r => r.GetItemById(It.IsAny<int>())).Returns(Task.FromResult(_item4));

            _catalogueItemRepo.Setup(r => r.CreateCatalogueItem(It.IsAny<CatalogueItem>())).Returns(Task.FromResult(EntityState.Unchanged));
            // OR:
            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(0);

            _mapper.Setup(m => m.Map<CatalogueItem>(It.IsAny<Item>())).Returns(_newCatalogueItem);

            _mapper.Setup(m => m.Map<CatalogueItem>(It.IsAny<CatalogueItemCreateDTO>())).Returns(_newCatalogueItem);


            var result = _catalogueItemService.CreateCatalogueItem(_item1.Id, _catalogueItemCreateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with item id '{_catalogueItem1.ItemId}' was NOT created !"));
        }



        //  AddExtrasToCatalogueItem()

        [Test]
        public void AddExtrasToCatalogueItem_WhenCalled_AddsExtrasToCatalogueItem()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(true));

            _catalogueItemRepo.Setup(r => r.AddAccessoriesToCatalogueItem(It.IsAny<int>(), _accessoryItems)).Returns(Task.CompletedTask);

            _catalogueItemRepo.Setup(r => r.AddSimilarProductsToCatalogueItem(It.IsAny<int>(), _similarProductItems)).Returns(Task.CompletedTask);

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(1);


            var result = _catalogueItemService.AddExtrasToCatalogueItem(_catalogueItem1.ItemId, _extrasAddDTO).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Accessories.Count(), Is.EqualTo(_extrasAddDTO.AccessoryIds.Count()));
            Assert.That(result.Data.Accessories.ElementAt(1), Is.EqualTo(_extrasAddDTO.AccessoryIds.ElementAt(1)));
            Assert.That(result.Data.SimilarProducts.Count(), Is.EqualTo(_extrasAddDTO.SimilarProductIds.Count()));
            Assert.That(result.Data.SimilarProducts.ElementAt(0), Is.EqualTo(_extrasAddDTO.SimilarProductIds.ElementAt(0)));
            _catalogueItemRepo.Verify(r => r.SaveChanges());
        }



        [Test]
        public void AddExtrasToCatalogueItem_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));


            var result = _catalogueItemService.AddExtrasToCatalogueItem(_catalogueItem1.ItemId, _extrasAddDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with id '{_catalogueItem1.ItemId}' NOT found !"));
        }



        [Test]
        public void AddExtrasToCatalogueItem_ExtrasWasNotAdded_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(true));

            _catalogueItemRepo.Setup(r => r.AddAccessoriesToCatalogueItem(It.IsAny<int>(), _accessoryItems)).Returns(Task.CompletedTask);

            _catalogueItemRepo.Setup(r => r.AddSimilarProductsToCatalogueItem(It.IsAny<int>(), _similarProductItems)).Returns(Task.CompletedTask);

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(0);


            var result = _catalogueItemService.AddExtrasToCatalogueItem(_catalogueItem1.ItemId, _extrasAddDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Extras for catalogue item with item id '{_catalogueItem1.ItemId}' were NOT added !"));
            _catalogueItemRepo.Verify(r => r.SaveChanges());
        }



        //  UpdateCatalogueItem()

        [Test]
        public void UpdateCatalogueItem_WhenCalled_UpdatesCatalogueItem()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_updatedCatalogueItem));

            _mapper.Setup(m => m.Map<CatalogueItem>(_catalogueItemUpdateDTO)).Returns(_updatedCatalogueItem);

            _mapper.Setup(m => m.Map<CatalogueItemReadDTO>(_updatedCatalogueItem)).Returns(_updatedCatalogueItemReadDTO);

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(1);


            var result = _catalogueItemService.UpdateCatalogueItem(_item5_Id, _catalogueItemUpdateDTO).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ItemId, Is.EqualTo(_item5_Id));
            _catalogueItemRepo.Verify(r => r.SaveChanges());
        }


        [Test]
        public void UpdateCatalogueItem_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));


            var result = _catalogueItemService.UpdateCatalogueItem(_item5_Id, _catalogueItemUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with id '{_item5_Id}' NOT found !"));
        }


        [Test]
        public void UpdateCatalogueItem_CatalogueItemNotUpdated_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_updatedCatalogueItem));

            _mapper.Setup(m => m.Map<CatalogueItem>(_catalogueItemUpdateDTO)).Returns(_updatedCatalogueItem);

            _mapper.Setup(m => m.Map<CatalogueItemReadDTO>(_updatedCatalogueItem)).Returns(_updatedCatalogueItemReadDTO);

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(0);


            var result = _catalogueItemService.UpdateCatalogueItem(_item5_Id, _catalogueItemUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item '{_item5_Id}': changes were NOT saved into DB !"));
            _catalogueItemRepo.Verify(r => r.SaveChanges());
        }



        //  RemoveCatalogueItem()

        [Test]
        public void RemoveCatalogueItem_WhenCalled_RemovesCatalogueItemFromRepo()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.RemoveCatalogueItem(_catalogueItem1)).Returns(Task.FromResult(EntityState.Deleted));
            // OR:
            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(1);


            var result = _catalogueItemService.RemoveCatalogueItem(_catalogueItem1.ItemId).Result;


            Assert.IsTrue(result.Status);
            _catalogueItemRepo.Verify(r => r.SaveChanges());
        }


        [Test]
        public void RemoveCatalogueItem_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));


            var result = _catalogueItemService.RemoveCatalogueItem(_catalogueItem1.ItemId).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with id '{_catalogueItem1.ItemId}' was NOT found !"));
        }


        [Test]
        public void RemoveCatalogueItem_CatalogueItemNotRemoved_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.RemoveCatalogueItem(_catalogueItem1)).Returns(Task.FromResult(EntityState.Unchanged));
            // OR:
            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(0);

            var result = _catalogueItemService.RemoveCatalogueItem(_catalogueItem1.ItemId).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with item id '{_catalogueItem1.ItemId}' was NOT removed !"));
        }



        // RemoveExtrasFromCatalogueItem()

        [Test]
        public void RemoveExtrasFromCatalogueItem_WhenCalled_RemovesExtrasFromCatalogueItem()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(_item1_Id)).Returns(Task.FromResult(_catalogueItem1WithExtras));

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(1);

            var extrasRemoveDTO = new ExtrasRemoveDTO
            {
                AccessoryIds = new List<int> { _accessoryItem1.AccessoryItemId },
                SimilarProductIds = new List<int> { _similarProductItem1.SimilarProductItemId }
            };


            var result = _catalogueItemService.RemoveExtrasFromCatalogueItem(_catalogueItem1.ItemId, extrasRemoveDTO).Result;


            Assert.IsTrue(result.Status);
            _catalogueItemRepo.Verify(r => r.RemoveAccessoriesFromCatalogueItem(_item1_Id, _accessoryItems));
            _catalogueItemRepo.Verify(r => r.RemoveSimilarProductsFromCatalogueItem(_item1_Id, _similarProductItems));
            Assert.That(result.Data.Accessories.ElementAt(0), Is.EqualTo(_catalogueItem1WithExtras.Accessories.ElementAt(0).AccessoryItemId));
        }


        [Test]
        public void RemoveExtrasFromCatalogueItem_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(_item1_Id)).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));


            var result = _catalogueItemService.RemoveExtrasFromCatalogueItem(_catalogueItem1.ItemId, It.IsAny<ExtrasRemoveDTO>()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item with id '{_catalogueItem1.ItemId}' NOT found !"));
        }


        [Test]
        public void RemoveExtrasFromCatalogueItem_ExtrasNotRemoved_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemWithExtrasById(_item1_Id)).Returns(Task.FromResult(_catalogueItem1WithExtras));

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(0);

            var extrasRemoveDTO = new ExtrasRemoveDTO
            {
                AccessoryIds = new List<int> { _accessoryItem1.AccessoryItemId },
                SimilarProductIds = new List<int> { _similarProductItem1.SimilarProductItemId }
            };


            var result = _catalogueItemService.RemoveExtrasFromCatalogueItem(_catalogueItem1.ItemId, extrasRemoveDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Extras for catalogue item with item id '{_catalogueItem1.ItemId}' were NOT removed !"));
        }



        // GetInstockCount()

        [Test]
        public void GetInstockCount_WhenCalled_ReturnsItemStockAmount()
        {
            var _amount = 123;

            _catalogueItemRepo.Setup(r => r.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(true));

            _itemRepo.Setup(r => r.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(true));

            _catalogueItemRepo.Setup(r => r.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_amount));


            var result = _catalogueItemService.GetInstockCount(_item1_Id).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.EqualTo(_amount));
        }


        [Test]
        public void GetInstockCount_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.ExistsById(_item1_Id)).Returns(Task.FromResult(false));

            _itemRepo.Setup(r => r.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(false));

            var result = _catalogueItemService.GetInstockCount(_item1_Id).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue Item with id '{_item1_Id}' NOT found"));
        }


        [Test]
        public void GetInstockCount_CatalogueItemNotFoundButItemExists_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(false));

            _itemRepo.Setup(r => r.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(true));


            var result = _catalogueItemService.GetInstockCount(_item1_Id).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue Item with id '{_item1_Id}' NOT found, but Item with id '{_item1_Id}' exists and it's not registered in catalogue !"));
        }



        // RemoveFromStockAmount()

        [Test]
        public void RemoveFromStockAmount_WhenCalled_ReducesAmountCatalogueItemFromStock()
        {
            var _amount = 123;
            var _amountToRemove = 23;
            _catalogueItem1.Instock = _amount;

            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1.Instock));

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(1);


            var result = _catalogueItemService.RemoveFromStockAmount(_item1_Id, _amountToRemove).Result;



            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.EqualTo(_amount - _amountToRemove));
        }


        [Test]
        public void RemoveFromStockAmount_AmountToRemoveExceededAmountInStock_StockAmountIsZeroAndReturnsMessage()
        {
            var _amount = 100;
            var _amountToRemove = 123;
            _catalogueItem1.Instock = _amount;

            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1.Instock));

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(1);


            var result = _catalogueItemService.RemoveFromStockAmount(_item1_Id, _amountToRemove).Result;



            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo($"Insufficient amount in stock ! Only {_amount} catalogue items were removed from stock !"));
        }


        [Test]
        public void RemoveFromStockAmount_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(_item1_Id)).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));

            _itemRepo.Setup(r => r.ExistsById(_item1_Id)).Returns(Task.FromResult(false));


            var result = _catalogueItemService.RemoveFromStockAmount(_item1_Id, It.IsAny<int>()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue Item with id '{_item1_Id}' NOT found !"));
        }


        [Test]
        public void RemoveFromStockAmount_CatalogueItemNotFoundButItemExists_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(_item1_Id)).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));

            _itemRepo.Setup(r => r.ExistsById(_item1_Id)).Returns(Task.FromResult(true));


            var result = _catalogueItemService.RemoveFromStockAmount(_item1_Id, It.IsAny<int>()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue Item with id '{_item1_Id}' NOT found, but Item with this id exists !"));
        }


        [Test]
        public void RemoveFromStockAmount_CatalogueItemNotRemoved_ReturnsMessage()
        {
            var _amountToRemove = 23;
            _catalogueItem1.Instock = 123;

            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1.Instock));

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(0);


            var result = _catalogueItemService.RemoveFromStockAmount(_item1_Id, _amountToRemove).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item '{_item1_Id}' instock amount was NOT changed !"));
        }



        // AddAmountToStock()

        [Test]
        public void AddAmountToStock_WhenCalled_AddsAmountCatalogueItemToStock()
        {
            var _amount = 100;
            var _amountToRemove = 23;
            _catalogueItem1.Instock = _amount;

            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1.Instock));

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(1);


            var result = _catalogueItemService.AddAmountToStock(_item1_Id, _amountToRemove).Result;



            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.EqualTo(_amount + _amountToRemove));
        }


        [Test]
        public void AddAmountToStock_CatalogueItemNotFound_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(_item1_Id)).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));

            _itemRepo.Setup(r => r.ExistsById(_item1_Id)).Returns(Task.FromResult(false));


            var result = _catalogueItemService.AddAmountToStock(_item1_Id, It.IsAny<int>()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue Item with id '{_item1_Id}' NOT found !"));
        }


        [Test]
        public void AddAmountToStock_CatalogueItemNotFoundButItemExists_ReturnsMessage()
        {
            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(_item1_Id)).Returns(Task.FromResult(It.IsAny<CatalogueItem>()));

            _itemRepo.Setup(r => r.ExistsById(_item1_Id)).Returns(Task.FromResult(true));


            var result = _catalogueItemService.AddAmountToStock(_item1_Id, It.IsAny<int>()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue Item with id '{_item1_Id}' NOT found, but Item with this id exists !"));
        }


        [Test]
        public void AddAmountToStock_CatalogueItemNotRemoved_ReturnsMessage()
        {
            var _amountToRemove = 23;
            _catalogueItem1.Instock = 100;

            _catalogueItemRepo.Setup(r => r.GetCatalogueItemById(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1));

            _catalogueItemRepo.Setup(r => r.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_catalogueItem1.Instock));

            _catalogueItemRepo.Setup(r => r.SaveChanges()).Returns(0);


            var result = _catalogueItemService.AddAmountToStock(_item1_Id, _amountToRemove).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Catalogue item '{_item1_Id}' instock amount was NOT changed !"));
        }

    }
}
