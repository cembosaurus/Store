using AutoMapper;
using Business.Inventory.DTOs.Item;
using Inventory.Models;
using Business.Libraries.ServiceResult;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Services.Inventory.Data.Repositories.Interfaces;

namespace Store.Test.Services.Inventory.Services
{
    [TestFixture]
    internal class ItemService_Test
    {
        private IItemService _itemService;

        private Mock<IItemRepository> _repo = new Mock<IItemRepository>();
        private Mock<IMapper> _mapper = new Mock<IMapper>();

        private int _item1_Id = 1, _item2_Id = 2, _item3_Id = 3, _newItem_Id = 4, _itemToUpdate_Id = 1;
        private Item _item1, _item2, _item3, _newItem, _itemToUpdate, _itemToDelete;
        private ItemReadDTO _item1ReadDTO, _item2ReadDTO, _item3ReadDTO, _newItemReadDTO, _updatedItemReadDTO, _deletedItemReadDTO;
        private ItemCreateDTO _itemToCreateDTO = new ItemCreateDTO { 
            Name = "Item To Create", Description = "Description of Item to create", PhotoURL = "Photo URL of Item to create" 
        };
        private ItemUpdateDTO _itemToUpdateDTO = new ItemUpdateDTO { 
            Name = "Item To Update", Description = "Description of Item to update", PhotoURL = "Photo URL of Item to update" 
        };
        private List<Item> _items;
        private IEnumerable<ItemReadDTO> _itemReadDTO_List;


        [SetUp]
        public void Setup()
        {
            _item1 = new Item{ Id = _item1_Id, Name = "Item 1", Description = "Description of Item 1", PhotoURL = "photo url of Item 1", Archived = false};
            _item2 = new Item{ Id = _item2_Id, Name = "Item 2", Description = "Description of Item 2", PhotoURL = "photo url of Item 2", Archived = true};
            _item3 = new Item{ Id = _item3_Id, Name = "Item 3", Description = "Description of Item 3", PhotoURL = "photo url of Item 3", Archived = true};
            _items = new List<Item> { _item1, _item2, _item3};

            _item1ReadDTO = new ItemReadDTO { Id = _item1.Id, Name = _item1.Name, Description = _item1.Description, PhotoURL = _item1.PhotoURL };
            _item2ReadDTO = new ItemReadDTO { Id = _item2.Id, Name = _item2.Name, Description = _item2.Description, PhotoURL = _item2.PhotoURL };
            _item3ReadDTO = new ItemReadDTO { Id = _item3.Id, Name = _item3.Name, Description = _item3.Description, PhotoURL = _item3.PhotoURL };
            _itemReadDTO_List = new List<ItemReadDTO> { _item1ReadDTO, _item2ReadDTO, _item3ReadDTO };

            _newItem = new Item { Id = _newItem_Id, Name = _itemToCreateDTO.Name, Description = _itemToCreateDTO.Description, PhotoURL = _itemToCreateDTO.PhotoURL};
            _newItemReadDTO = new ItemReadDTO { Id = _newItem.Id, Name = _newItem.Name, Description = _newItem.Description, PhotoURL = _newItem.PhotoURL };

            _itemToUpdate = new Item { Id = _itemToUpdate_Id, Name = _itemToUpdateDTO.Name, Description = _itemToUpdateDTO.Description, PhotoURL = _itemToUpdateDTO.PhotoURL };
            _updatedItemReadDTO = new ItemReadDTO { Id = _itemToUpdate.Id, Name = _itemToUpdate.Name, Description = _itemToUpdate.Description, PhotoURL = _itemToUpdate.PhotoURL };

            _itemToDelete = _item1;
            _deletedItemReadDTO = new ItemReadDTO { Id = _itemToDelete.Id, Name = _itemToDelete.Name, Description = _itemToDelete.Description, PhotoURL = _itemToDelete.PhotoURL };


            _itemService = new ItemService(_repo.Object, _mapper.Object, new ServiceResultFactory());
        }




        //  GetItems()

        [Test]
        public void GetItems_WhenCalled_ReturnsItems()
        {
            _repo.Setup(r => r.GetItems(It.IsAny<IEnumerable<int>>())).Returns(Task.FromResult(_items.AsEnumerable()));

            _mapper.Setup(m => m.Map<IEnumerable<ItemReadDTO>>(It.IsAny<IEnumerable<Item>>())).Returns(_itemReadDTO_List);


            var result = _itemService.GetItems().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_itemReadDTO_List.Count()));
            Assert.That(result.Data.ElementAt(2), Is.EqualTo(_itemReadDTO_List.ElementAt(2)));
        }


        [Test]
        public void GetItems_WhenCalledWithParameter_ReturnsItems()
        {
            IEnumerable<int> itemIdsArgument = new List<int> { _item1.Id, _item2.Id };
            IEnumerable<Item> itemsFromRepo = new List<Item> { _item1, _item2 };
            IEnumerable<ItemReadDTO> itemReadDTOsFromRepo = new List<ItemReadDTO> { _item1ReadDTO, _item2ReadDTO };

            _repo.Setup(r => r.GetItems(itemIdsArgument)).Returns(Task.FromResult(itemsFromRepo));

            _mapper.Setup(m => m.Map<IEnumerable<ItemReadDTO>>(It.IsAny<IEnumerable<Item>>())).Returns(itemReadDTOsFromRepo);


            var result = _itemService.GetItems().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(itemsFromRepo.Count()));
            Assert.That(result.Data.ElementAt(1).Id, Is.EqualTo(itemsFromRepo.ElementAt(1).Id));
        }


        [Test]
        public void GetItems_NoItemsFound_ReturnsFailMessage()
        {
            _repo.Setup(r => r.GetItems(It.IsAny<IEnumerable<int>>())).Returns(Task.FromResult(new List<Item>().AsEnumerable()));

            _mapper.Setup(m => m.Map<IEnumerable<ItemReadDTO>>(It.IsAny<IEnumerable<Item>>())).Returns(It.IsAny<IEnumerable<ItemReadDTO>>());


            var result = _itemService.GetItems().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo("NO items found !"));
        }



        //  GetItemById()

        [Test]
        public void GetItemById_WhenCalled_ReturnsItem()
        {
            _repo.Setup(r => r.GetItemById(It.IsAny<int>())).Returns(Task.FromResult(_item1));

            _mapper.Setup(m => m.Map<ItemReadDTO>(It.IsAny<Item>())).Returns(_item1ReadDTO);


            var result = _itemService.GetItemById(It.IsAny<int>()).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Id, Is.EqualTo(_item1_Id));
        }


        [Test]
        public void GetItemById_ItemNotFound_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<Item>()));

            _mapper.Setup(m => m.Map<ItemReadDTO>(It.IsAny<Item>())).Returns(It.IsAny<ItemReadDTO>());


            var result = _itemService.GetItemById(_item1_Id).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item '{_item1_Id}' was NOT found !"));
        }


        //  AddItem()

        [Test]
        public void AddItem_WhenCalled_AddsItemToRepo()
        {
            _repo.Setup(r => r.ExistsByName(_itemToCreateDTO.Name)).Returns(Task.FromResult(false));

            _repo.Setup(r => r.AddItem(It.IsAny<Item>())).Returns(Task.FromResult(EntityState.Added));

            _repo.Setup(r => r.SaveChanges()).Returns(1);

            _mapper.Setup(m => m.Map<ItemReadDTO>(It.IsAny<Item>())).Returns(_newItemReadDTO);


            var result = _itemService.AddItem(_itemToCreateDTO).Result;


            _repo.Verify(r => r.AddItem(It.IsAny<Item>()));
            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Id, Is.EqualTo(_newItem.Id));
            Assert.That(result.Data.Name, Is.EqualTo(_itemToCreateDTO.Name));
        }


        [Test]
        public void AddItem_AlreadyExists_ReturnMessage()
        {
            _repo.Setup(r => r.ExistsByName(_itemToCreateDTO.Name)).Returns(Task.FromResult(true));


            var result = _itemService.AddItem(_itemToCreateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item '{_itemToCreateDTO.Name}' already EXISTS !"));
        }


        [Test]
        public void AddItem_ItemNotCreated_ReturnMessage()
        {
            _repo.Setup(r => r.ExistsByName(_itemToCreateDTO.Name)).Returns(Task.FromResult(false));

            _repo.Setup(r => r.AddItem(It.IsAny<Item>())).Returns(Task.FromResult(EntityState.Unchanged));
            // OR:
            _repo.Setup(r => r.SaveChanges()).Returns(0);


            var result = _itemService.AddItem(_itemToCreateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item '{_itemToCreateDTO.Name}' was NOT created"));
        }



        //  UpdateItem()

        [Test]
        public void UpdateItem_WhenCalled_UpdatesItem()
        {
            _repo.Setup(r => r.GetItemById(_itemToUpdate_Id)).Returns(Task.FromResult(_item1));

            _repo.Setup(r => r.ExistsByName(_itemToUpdateDTO.Name)).Returns(Task.FromResult(false));

            _repo.Setup(r => r.SaveChanges()).Returns(1);

            _mapper.Setup(m => m.Map<Item>(_itemToUpdateDTO)).Returns(_item1);

            _mapper.Setup(m => m.Map<ItemReadDTO>(_item1)).Returns(_updatedItemReadDTO);


            var result = _itemService.UpdateItem(_itemToUpdate_Id, _itemToUpdateDTO).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Id, Is.EqualTo(_itemToUpdate_Id));
        }


        [Test]
        public void UpdateItem_ItemNotFound_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemById(_itemToUpdate_Id)).Returns(Task.FromResult(It.IsAny<Item>()));


            var result = _itemService.UpdateItem(_itemToUpdate_Id, _itemToUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item '{_itemToUpdate_Id}' NOT found !"));
        }


        [Test]
        public void UpdateItem_ItemNameAlreadyExists_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemById(_itemToUpdate_Id)).Returns(Task.FromResult(_item1));

            _repo.Setup(r => r.ExistsByName(_itemToUpdateDTO.Name)).Returns(Task.FromResult(true));


            var result = _itemService.UpdateItem(_itemToUpdate_Id, _itemToUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item with name: '{_itemToUpdateDTO.Name}' already exists !"));
        }


        [Test]
        public void UpdateItem_UpdateNotSaved_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemById(_itemToUpdate_Id)).Returns(Task.FromResult(_item1));

            _repo.Setup(r => r.ExistsByName(_itemToUpdateDTO.Name)).Returns(Task.FromResult(false));

            _repo.Setup(r => r.SaveChanges()).Returns(0);


            var result = _itemService.UpdateItem(_itemToUpdate_Id, _itemToUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item '{_itemToUpdate_Id}': changes were NOT saved into DB !"));
        }



        //  DeleteItem()

        [Test]
        public void DeleteItem_WhenCalled_DeletesItem()
        {
            _repo.Setup(r => r.GetItemById(_item1_Id)).Returns(Task.FromResult(_item1));

            _repo.Setup(r => r.DeleteItem(_item1)).Returns(Task.FromResult(EntityState.Deleted));
            // OR:
            _repo.Setup(r => r.SaveChanges()).Returns(1);

            _mapper.Setup(m => m.Map<ItemReadDTO>(_item1)).Returns(_deletedItemReadDTO);


            var result = _itemService.DeleteItem(_item1_Id).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Id, Is.EqualTo(_item1_Id));
        }



        [Test]
        public void DelteItem_ItemNotFound_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemById(_item1_Id)).Returns(Task.FromResult(It.IsAny<Item>()));


            var result = _itemService.DeleteItem(_item1_Id).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item '{_item1_Id}' NOT found !"));
        }


        [Test]
        public void DelteItem_ItemIsArchived_ReturnsMessage()
        {
            _item1.Archived = true;

            _repo.Setup(r => r.GetItemById(_item1_Id)).Returns(Task.FromResult(_item1));


            var result = _itemService.DeleteItem(_item1_Id).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item '{_item1_Id}' can NOT be deleted because it is ARCHIVED !"));
        }


        [Test]
        public void DelteItem_ItemNotDeleted_ReturnsMessage()
        {
            _repo.Setup(r => r.GetItemById(_item1_Id)).Returns(Task.FromResult(_item1));

            _repo.Setup(r => r.DeleteItem(_item1)).Returns(Task.FromResult(EntityState.Unchanged));
            // OR:
            _repo.Setup(r => r.SaveChanges()).Returns(0);

            _mapper.Setup(m => m.Map<ItemReadDTO>(_item1)).Returns(_deletedItemReadDTO);


            var result = _itemService.DeleteItem(_item1_Id).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Item with id '{_item1_Id}' was NOT removed from DB !"));
        }
    }
}
