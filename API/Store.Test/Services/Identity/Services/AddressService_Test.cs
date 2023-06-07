using AutoMapper;
using Business.Identity.DTOs;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.Data.Repositories.Interfaces;
using Identity.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Services.Identity.Models;
using System.Net;

namespace Store.Test.Services.Identity.Services
{
    [TestFixture]
    internal class AddressService_Test
    {

        private AddressService _addressService;
        private IServiceResultFactory _resultFact;

        private Mock<IAddressRepository> _addressRepo;
        private Mock<IUserRepository> _userRepo;
        private Mock<IMapper> _mapper;

        private int _addressId_1 = 1, _addressId_2 = 2, _addressId_3 = 3, _addressId_ToCreate = 4, _addressId_ToUpdate = 5, _addressId_NonExisting = 6;
        private string _addCity_1 = "Test City 1", _addCity_2 = "Test City 2", _addCity_3 = "Test City 3";
        private string _addStreet_1 = "Test Street 1", _addStreet_2 = "Test Street 2", _addStreet_3 = "Test Street 3";
        private int _number_1 = 11, _number_2 = 22, _number_3 = 33, _number_4 = 44, _number_5 = 55;
        private Address _address1, _address2, _address3;
        private AddressReadDTO _addressReadDTO1, _addressReadDTO2, _addressReadDTO3;
        private AddressCreateDTO _addressCreateDTO;
        private AddressUpdateDTO _addressUpdateDTO;
        private Address _addressCreate;
        private Address _addressUpdate;
        private IEnumerable<Address> _addresses;
        private IEnumerable<AddressReadDTO> _addressesReadDTO;
        private int _userId = 1;
        private AppUser _user;


        [SetUp]
        public void Setup()
        {
            _resultFact = new ServiceResultFactory();

            _addressRepo = new Mock<IAddressRepository>();
            _userRepo = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();

            _address1 = new Address { AddressId = _addressId_1, City = _addCity_1, Street = _addStreet_1, Number = _number_1 };
            _address2 = new Address { AddressId = _addressId_2, City = _addCity_2, Street = _addStreet_2, Number = _number_2 };
            _address3 = new Address { AddressId = _addressId_3, City = _addCity_3, Street = _addStreet_3, Number = _number_3 };
            _addresses = new List<Address> { _address1, _address2, _address3 };
            _addressReadDTO1 = new AddressReadDTO { AddressId = _addressId_1, City = _addCity_1, Street = _addStreet_1, Number = _number_1 };
            _addressReadDTO2 = new AddressReadDTO { AddressId = _addressId_2, City = _addCity_2, Street = _addStreet_2, Number = _number_2 };
            _addressReadDTO3 = new AddressReadDTO { AddressId = _addressId_3, City = _addCity_3, Street = _addStreet_3, Number = _number_3 };
            _addressesReadDTO = new List<AddressReadDTO> { _addressReadDTO1, _addressReadDTO2, _addressReadDTO3 };
            _addressCreateDTO = new AddressCreateDTO { City = "New City Test", Street = "New Street Test", Number = _number_4 };
            _addressUpdateDTO = new AddressUpdateDTO { City = "Updated City Test", Street = "Updated Street Test", Number = _number_5 };
            _addressCreate = new Address { AddressId = _addressId_ToCreate, City = _addressCreateDTO.City, Street = _addressCreateDTO.Street, Number = _addressCreateDTO.Number };
            _addressUpdate = new Address { AddressId = _addressId_ToUpdate, City = _addressUpdateDTO.City, Street = _addressUpdateDTO.Street, Number = _addressUpdateDTO.Number };

            _user = new AppUser { Id = _userId, UserName = "Test User", UserAddresses = new List<UserAddress>() };

            _addressService = new AddressService(_addressRepo.Object, _resultFact, _mapper.Object, _userRepo.Object);
        }




        //  GetAllAddresses()

        [Test]
        public void GetAllAddresses_WhenCalled_ReturnsListOfAddresses()
        {
            _addressRepo.Setup(ar => ar.GetAllAddresses()).Returns(Task.FromResult(_addresses));

            _mapper.Setup(m => m.Map<IEnumerable<AddressReadDTO>>(_addresses)).Returns(_addressesReadDTO);


            var result = _addressService.GetAllAddresses().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_addressesReadDTO.Count()));
            Assert.That(result.Data.ElementAt(0), Is.EqualTo(_addressesReadDTO.ElementAt(0)));
        }


        [Test]
        public void GetAllAddresses_NoAddressesInRepo_ReturnsMessage()
        {
            _addressRepo.Setup(ar => ar.GetAllAddresses()).Returns(Task.FromResult(new List<Address>().AsEnumerable()));

            _mapper.Setup(m => m.Map<IEnumerable<AddressReadDTO>>(_addresses)).Returns(_addressesReadDTO);


            var result = _addressService.GetAllAddresses().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo("NO addresses found !"));
        }



        //  GetAddressById()

        [Test]
        public void GetAddressById_WhenCalled_ReturnsAddress()
        {
            _addressRepo.Setup(ar => ar.GetAddressById(It.IsAny<int>())).Returns(Task.FromResult(_address1));

            _mapper.Setup(m => m.Map<AddressReadDTO>(_address1)).Returns(_addressReadDTO1);


            var result = _addressService.GetAddressById(_addressId_1).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.AddressId, Is.EqualTo(_addressReadDTO1.AddressId));
            Assert.That(result.Data.City, Is.EqualTo(_addressReadDTO1.City));
            Assert.That(result.Data.Street, Is.EqualTo(_addressReadDTO1.Street));
            Assert.That(result.Data.Number, Is.EqualTo(_addressReadDTO1.Number));
        }


        [Test]
        public void GetAddressById_NoAddressesInRepo_ReturnsMessage()
        {
            _addressRepo.Setup(ar => ar.GetAddressById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<Address>()));


            var result = _addressService.GetAddressById(_addressId_NonExisting).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.EqualTo(null));
            Assert.That(result.Message, Is.EqualTo($"Address '{_addressId_NonExisting}' NOT found !"));
        }


        //  GetAddressesByIds()

        [Test]
        public void GetAddressesByIds_WhenCalled_ReturnsListOfAddresses()
        {
            _addressRepo.Setup(ar => ar.GetAddressesByIds(It.IsAny<List<int>>())).Returns(Task.FromResult(_addresses));

            _mapper.Setup(m => m.Map<IEnumerable<AddressReadDTO>>(_addresses)).Returns(_addressesReadDTO);


            var result = _addressService.GetAddressesByIds(new List<int> { _addressId_1, _addressId_2, _addressId_3 }).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_addressesReadDTO.Count()));
            Assert.That(result.Data.ElementAt(0), Is.EqualTo(_addressesReadDTO.ElementAt(0)));
        }


        [Test]
        public void GetAddressesByIds_NoAddressesInRepo_ReturnsMessage()
        {
            _addressRepo.Setup(ar => ar.GetAddressesByIds(It.IsAny<List<int>>())).Returns(Task.FromResult(new List<Address>().AsEnumerable()));

            _mapper.Setup(m => m.Map<IEnumerable<AddressReadDTO>>(_addresses)).Returns(_addressesReadDTO);


            var result = _addressService.GetAddressesByIds(new List<int> { _addressId_NonExisting }).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo("NO addresses corresponding to given Ids were found !"));
        }



        //  GetAddressesByUserId()

        [Test]
        public void GetAddressesByUserId_WhenCalled_ReturnsAddress()
        {
            _userRepo.Setup(ur => ur.GetUserById(It.IsAny<int>())).Returns(Task.FromResult(_user));

            _addressRepo.Setup(ar => ar.GetAddressesByUserId(It.IsAny<int>())).Returns(Task.FromResult(_addresses));

            _mapper.Setup(m => m.Map<IEnumerable<AddressReadDTO>>(_addresses)).Returns(_addressesReadDTO);


            var result = _addressService.GetAddressesByUserId(_userId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_addressesReadDTO.Count()));
            Assert.That(result.Data.ElementAt(0), Is.EqualTo(_addressesReadDTO.ElementAt(0)));
        }


        [Test]
        public void GetAddressesByUserId_NoAddressesInRepo_ReturnsMessage()
        {
            _userRepo.Setup(ur => ur.GetUserById(It.IsAny<int>())).Returns(Task.FromResult(_user));

            _addressRepo.Setup(ar => ar.GetAddressesByUserId(It.IsAny<int>())).Returns(Task.FromResult(new List<Address>().AsEnumerable()));


            var result = _addressService.GetAddressesByUserId(_userId).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.Empty);
            Assert.That(result.Message, Is.EqualTo($"Addresses for user '{_userId}' NOT found !"));
        }


        [Test]
        public void GetAddressesByUserId_NoUserInRepo_ReturnsFailMessage()
        {
            _userRepo.Setup(ur => ur.GetUserById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<AppUser>()));

            _addressRepo.Setup(ar => ar.GetAddressesByUserId(It.IsAny<int>())).Returns(Task.FromResult(new List<Address>().AsEnumerable()));


            var result = _addressService.GetAddressesByUserId(_userId).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Message, Is.EqualTo($"User '{_userId}' NOT found !"));
        }



        // ExistsAddressById()

        [Test]
        public void ExistsAddressById_WhenAddressExists_ReturnsEmptyMessage()
        {
            _addressRepo.Setup(ar => ar.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(true));


            var result = _addressService.ExistsAddressById(_addressId_1).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo(""));
        }


        [Test]
        public void ExistsAddressById_WhenAddressDoesntExist_ReturnsNoAddressFoundMessage()
        {
            _addressRepo.Setup(ar => ar.ExistsById(It.IsAny<int>())).Returns(Task.FromResult(false));


            var result = _addressService.ExistsAddressById(_addressId_1).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Address '{_addressId_1}' does NOT exist !"));
        }



        // AddAddressToUser()

        [Test]
        public void AddAddressToUser_WhenCalled_AddsAddressToRepoAndreturnsAddressDTO()
        {
            _userRepo.Setup(ur => ur.GetUserById(It.IsAny<int>())).Returns(Task.FromResult(_user));

            _mapper.Setup(m => m.Map<Address>(_addressCreateDTO)).Returns(_addressCreate);

            _addressRepo.Setup(ar => ar.CreateAddress(_addressCreate)).Returns(Task.FromResult(EntityState.Added));

            _addressRepo.Setup(ar => ar.SaveChanges()).Returns(1);


            var result = _addressService.AddAddressToUser(_userId, _addressCreateDTO).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo(""));
            _addressRepo.Verify(r => r.SaveChanges());
        }



        [Test]
        public void AddAddressToUser_AddressNotSaved_ReturnsFailMessage()
        {
            _userRepo.Setup(ur => ur.GetUserById(_userId)).Returns(Task.FromResult(_user));

            _mapper.Setup(m => m.Map<Address>(_addressCreateDTO)).Returns(_addressCreate);

            _addressRepo.Setup(ar => ar.CreateAddress(_addressCreate)).Returns(Task.FromResult(EntityState.Added));

            _addressRepo.Setup(ar => ar.SaveChanges()).Returns(0);


            var result = _addressService.AddAddressToUser(_userId, _addressCreateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo("Address was NOT created"));
        }



        [Test]
        public void AddAddressToUser_NonExistingUser_ReturnsFailMessage()
        {
            _userRepo.Setup(ur => ur.GetUserById(_userId)).Returns(Task.FromResult(It.IsAny<AppUser>()));

            _mapper.Setup(m => m.Map<Address>(_addressCreateDTO)).Returns(_addressCreate);

            _addressRepo.Setup(ar => ar.CreateAddress(_addressCreate)).Returns(Task.FromResult(EntityState.Added));

            _addressRepo.Setup(ar => ar.SaveChanges()).Returns(1);


            var result = _addressService.AddAddressToUser(_userId, _addressCreateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"User '{_userId}' NOT found !"));
        }



        [Test]
        public void AddAddressToUser_NoAddressDTOInParameters_ReturnsFailMessage()
        {
            _userRepo.Setup(ur => ur.GetUserById(_userId)).Returns(Task.FromResult(_user));

            _mapper.Setup(m => m.Map<Address>(_addressCreateDTO)).Returns(_addressCreate);

            _addressRepo.Setup(ar => ar.CreateAddress(_addressCreate)).Returns(Task.FromResult(EntityState.Added));

            _addressRepo.Setup(ar => ar.SaveChanges()).Returns(1);


            var result = _addressService.AddAddressToUser(_userId, It.IsAny<AddressCreateDTO>()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"No address was entered !"));
        }



        //  UpdateAddress()

        [Test]
        public void UpdateAddress_WhenCalled_SavesChangesInRepo()
        {
            _addressRepo.Setup(ar => ar.GetAddressById(It.IsAny<int>())).Returns(Task.FromResult(_address1));

            _addressRepo.Setup(ar => ar.SaveChanges()).Returns(1);

            _mapper.Setup(m => m.Map<Address>(It.IsAny<AddressUpdateDTO>()))
                .Returns(() => { _addressUpdate.AddressId = _address1.AddressId; return _addressUpdate; });

            _mapper.Setup(m => m.Map<AddressReadDTO>(It.IsAny<Address>()))
                .Returns(new AddressReadDTO { 
                    AddressId = _address1.AddressId, City = _addressUpdate.City, Street = _addressUpdate.Street, Number = _addressUpdate.Number 
                });


            var result = _addressService.UpdateAddress(_addressId_1, _addressUpdateDTO).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.City, Is.EqualTo(_addressUpdateDTO.City));
            _addressRepo.Verify(r => r.SaveChanges());
        }


        [Test]
        public void UpdateAddress_AddressNotProvided_ReturnsFailMessage()
        {
            var result = _addressService.UpdateAddress(It.IsAny<int>(), It.IsAny<AddressUpdateDTO>()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo("Address was NOT provided !"));
        }

        [Test]
        public void UpdateAddress_AdressDetailsIncorrect_ReturnsFailMessage()
        {
            var result = _addressService.UpdateAddress(It.IsAny<int>(), new AddressUpdateDTO()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo("Correct address details was NOT provided !"));
        }

        [Test]
        public void UpdateAddress_AddressNotFound_ReturnsFailMessage()
        {
            _addressRepo.Setup(ar => ar.GetAddressById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<Address>()));


            var result = _addressService.UpdateAddress(It.IsAny<int>(), _addressUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo("Address NOT found !"));
        }

        [Test]
        public void UpdateAddress_AddressUpdateNotSaved_ReturnsFailMessage()
        {
            _addressRepo.Setup(ar => ar.GetAddressById(It.IsAny<int>())).Returns(Task.FromResult(_address1));

            _addressRepo.Setup(ar => ar.SaveChanges()).Returns(0);

            _mapper.Setup(m => m.Map<Address>(It.IsAny<AddressUpdateDTO>()))
                .Returns(() => { _addressUpdate.AddressId = _address1.AddressId; return _addressUpdate; });

            _mapper.Setup(m => m.Map<AddressReadDTO>(It.IsAny<Address>()))
                .Returns(new AddressReadDTO
                {
                    AddressId = _address1.AddressId,
                    City = _addressUpdate.City,
                    Street = _addressUpdate.Street,
                    Number = _addressUpdate.Number
                });


            var result = _addressService.UpdateAddress(_addressId_1, _addressUpdateDTO).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Address: '{_address1.AddressId}' was NOT updated !"));
            _addressRepo.Verify(r => r.SaveChanges());
        }



        //  DeleteAddress()

        [Test]
        public void DeleteAddress_WhenCalled_AddressDeleted()
        {
            _addressRepo.Setup(ar => ar.GetAddressById(It.IsAny<int>())).Returns(Task.FromResult(_address1));

            _addressRepo.Setup(ar => ar.DeleteAddress(It.IsAny<Address>())).Returns(Task.FromResult(EntityState.Deleted));

            _addressRepo.Setup(ar => ar.SaveChanges()).Returns(1);

            _mapper.Setup(m => m.Map<AddressReadDTO>(It.IsAny<Address>()))
                .Returns(new AddressReadDTO
                {
                    AddressId = _address1.AddressId,
                    City = _address1.City,
                    Street = _address1.Street,
                    Number = _address1.Number
                });


            var result = _addressService.DeleteAddress(_addressId_1).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.AddressId, Is.EqualTo(_address1.AddressId));
            _addressRepo.Verify(r => r.SaveChanges());
        }



        [Test]
        public void DeleteAddress_AddressNotFound_ReturnsFailMessage()
        {
            _addressRepo.Setup(ar => ar.GetAddressById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<Address>()));


            var result = _addressService.DeleteAddress(_addressId_1).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Address with Id '{_addressId_1}' NOT found !"));
        }


        [Test]
        public void DeleteAddress_AddressNotDeleted_ReturnsFailMessage()
        {
            _addressRepo.Setup(ar => ar.GetAddressById(It.IsAny<int>())).Returns(Task.FromResult(_address1));

            _addressRepo.Setup(ar => ar.DeleteAddress(It.IsAny<Address>())).Returns(Task.FromResult(EntityState.Unchanged));
            // OR:
            _addressRepo.Setup(ar => ar.SaveChanges()).Returns(0);

            _mapper.Setup(m => m.Map<AddressReadDTO>(It.IsAny<Address>()))
                .Returns(new AddressReadDTO
                {
                    AddressId = _address1.AddressId,
                    City = _address1.City,
                    Street = _address1.Street,
                    Number = _address1.Number
                });


            var result = _addressService.DeleteAddress(_addressId_1).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Address with id '{_addressId_1}' was NOT removed from DB !"));
        }

    }
}
