using AutoMapper;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Ordering.Data.Repositories.Interfaces;
using Ordering.HttpServices.Interfaces;
using Ordering.Services.Interfaces;

namespace Ordering.Services
{
    public class ArchiveService : IArchiveService
    {

        private readonly IArchiveRepository _archiveRepo;
        private readonly IHttpAddressService _httpIdentityService;
        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;

        public ArchiveService(IArchiveRepository archiveRepo, IServiceResultFactory resultFact, IMapper mapper, IHttpAddressService httpIdentityService)
        {
            _archiveRepo = archiveRepo;
            _httpIdentityService = httpIdentityService;
            _resultFact = resultFact;
            _mapper = mapper;
        }





        public async Task<IServiceResult<IEnumerable<OrderReadDTO>>> GetAllOrders()
        {
            var message = "";

            Console.WriteLine($"--> GETTING archived orders ......");


            var orders = await _archiveRepo.GetAllOrders();

            if (!orders.Any())
                return _resultFact.Result<IEnumerable<OrderReadDTO>>(null, true, "NO archived orders were found !");

            var result = _mapper.Map<IEnumerable<OrderReadDTO>>(orders);


            Console.WriteLine($"--> GETTING addresses for archived orders ......");


            var addressesIds = orders.Select(o => o.OrderDetails.AddressId);

            var addressesResult = await _httpIdentityService.GetAddressesByAddressIds(addressesIds);

            if (addressesResult == null || !addressesResult.Status || addressesResult.Data == null)
            {
                message += Environment.NewLine + $"Failed to obtain addresses for orders from service ! Reason: '{addressesResult?.Message ?? ""}'";
            }
            else
            {
                var addresses = addressesResult.Data;

                var orderWitAddressList = orders.Select(o => new
                {
                    o.CartId,
                    Address = addresses.FirstOrDefault(a => a.AddressId == o.OrderDetails.AddressId)
                });

                foreach (var o in result)
                {
                    o.OrderDetails.Address = orderWitAddressList.FirstOrDefault(owa => owa.CartId.Equals(o.CartId))!.Address;
                }
            }      

            return _resultFact.Result(result, true, message);
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByUserId(int userId)
        {
            var cartId = await _archiveRepo.GetCartIdByUserId(userId);

            if (cartId == null || cartId == Guid.Empty)
                return _resultFact.Result<OrderReadDTO>(null, true, $"Cart for Order with user Id '{userId}' does NOT exist !");


            var message = string.Empty;

            Console.WriteLine($"--> GETTING archived order '{userId}' ......");


            var order = await _archiveRepo.GetOrderByCartId(cartId);

            if (order == null)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Archived order '{userId}' was NOT found !");

            var result = _mapper.Map<OrderReadDTO>(order);


            Console.WriteLine($"--> GETTING address for order of user '{userId}' ......");


            var addressResult = await _httpIdentityService.GetAddressByAddressId(order.OrderDetails.AddressId);


            if (addressResult == null || !addressResult.Status || addressResult.Data == null)
            {
                message += Environment.NewLine + $"Failed to obtain address for order of user '{userId}' from service ! Reason: '{addressResult?.Message ?? ""}'";
            }
            else
            {
                result.OrderDetails.Address = addressResult.Data;
            }

            return _resultFact.Result(result, true, message);
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByCartId(Guid cartId)
        {
            var message = "";

            Console.WriteLine($"--> GETTING archived order '{cartId}' ......");


            var order = await _archiveRepo.GetOrderByCartId(cartId);

            if (order == null)
                return _resultFact.Result<OrderReadDTO>(null, true, $"Archived order with Cart Id: '{cartId}' was NOT found !");


            var result = _mapper.Map<OrderReadDTO>(order);


            Console.WriteLine($"--> GETTING address for order with cart Id '{cartId}' ......");


            var addressResult = await _httpIdentityService.GetAddressByAddressId(order.OrderDetails.AddressId);


            if (addressResult == null || !addressResult.Status || addressResult.Data == null)
            {
                message += Environment.NewLine + $"Failed to obtain address for order with cart Id '{cartId}' from service ! Reason: '{addressResult?.Message ?? ""}'";
            }
            else
            {
                result.OrderDetails.Address = addressResult.Data;
            }

            return _resultFact.Result(result, true, message);
        }


        public async Task<IServiceResult<OrderReadDTO>> GetOrderByOrderCode(string code)
        {
            var message ="";

            Console.WriteLine($"--> GETTING archived order '{code}' ......");


            var order = await _archiveRepo.GetOrderByOrderCode(code);

            if (order == null)
                return _resultFact.Result<OrderReadDTO>(null, true, $"Archived Order '{code}' was NOT found !");

            var result = _mapper.Map<OrderReadDTO>(order);


            Console.WriteLine($"--> GETTING address for order '{code}' ......");


            var addressResult = await _httpIdentityService.GetAddressByAddressId(order.OrderDetails.AddressId);


            if (addressResult == null || !addressResult.Status || addressResult.Data == null)
            {
                message += Environment.NewLine + $"Failed to obtain address for order '{code}' from service ! Reason: '{addressResult?.Message ?? ""}'";
            }
            else
            {
                result.OrderDetails.Address = addressResult.Data;
            }

            return _resultFact.Result(result, true, message);
        }


        public async Task<IServiceResult<IEnumerable<Guid>>> DeleteOrdersPermanently(int userId)
        {
            Console.WriteLine($"--> DELETING archived orders and carts for user '{userId}'......");


            var carts = await _archiveRepo.GetCartsByUserId(userId);

            if (carts == null || !carts.Any())
                return _resultFact.Result<IEnumerable<Guid>>(null, false, $"Carts and orders for user '{userId}' NOT found in archive !");

            await _archiveRepo.DeleteCartsPermanently(carts);

            if (_archiveRepo.SaveChanges() < 1)
                return _resultFact.Result<IEnumerable<Guid>>(null, false, $"Carts and orders for user '{userId}' was NOT permanently deleted from archive !");


            return _resultFact.Result(carts.Select(c => c.CartId), true);
        }




    }
}
