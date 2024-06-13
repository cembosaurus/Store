using AutoMapper;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;
using Business.Payment.Http.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ordering.Data.Repositories.Interfaces;
using Ordering.Services.Interfaces;
using Ordering.Tools.Interfaces;
using Services.Ordering.Models;



namespace Ordering.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepo;
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemsRepository _cartItemsRepo;
        private readonly IHttpAddressService _httpIdentityService;
        private readonly IHttpPaymentService _httpPaymentService;
        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;
        private readonly IOrder _orderTools;

        public OrderService(IOrderRepository orderRepo, ICartRepository cartRepo, ICartItemsRepository cartItemsRepo, IServiceResultFactory resultFact, IMapper mapper, IHttpAddressService httpIdentityService, IOrder orderTools, IHttpPaymentService httpPaymentService)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _cartItemsRepo = cartItemsRepo;
            _httpIdentityService = httpIdentityService;
            _httpPaymentService = httpPaymentService;
            _resultFact = resultFact;
            _mapper = mapper;
            _orderTools = orderTools;
        }





        public async Task<IServiceResult<IEnumerable<OrderReadDTO>>> GetAllOrders()
        {
            var message = "";

            Console.WriteLine($"--> GETTING orders ......");


            var orders = await _orderRepo.GetAllOrders();

            if (!orders.Any())
                return _resultFact.Result<IEnumerable<OrderReadDTO>>(null, true, "NO orders were found !");

            var result = _mapper.Map<IEnumerable<OrderReadDTO>>(orders);


            Console.WriteLine($"--> GETTING addresses form orders ......");


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
                    CartId = o.CartId,
                    Address = addresses.FirstOrDefault(a => a.AddressId == o.OrderDetails.AddressId)
                });

                foreach (var o in result)
                {
                    o.OrderDetails.Address = orderWitAddressList.FirstOrDefault(o => o.CartId == o.CartId).Address;
                }

            }

            return _resultFact.Result(result, true, message);
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByUserId(int userId)
        {
            var cartId = await _cartRepo.GetCartIdByUserId(userId);

            if (cartId == Guid.Empty)
                return _resultFact.Result<OrderReadDTO>(null, true, $"Cart for Order with user Id '{userId}' does NOT exist !");


            Console.WriteLine($"--> GETTING order '{userId}' ......");

            var message = "";


            var orderByCartId = await _orderRepo.GetOrderByCartId(cartId);

            if (orderByCartId == null)
                message = $"Order '{userId}' was NOT found !";

            return _resultFact.Result(_mapper.Map<OrderReadDTO>(orderByCartId), true, message);
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByCartId(Guid cartId)
        {
            Console.WriteLine($"--> GETTING order '{cartId}' ......");

            var message = "";


            var order = await _orderRepo.GetOrderByCartId(cartId);

            if (order == null)
                message = $"Order '{cartId}' was NOT found !";

            return _resultFact.Result(_mapper.Map<OrderReadDTO>(order), true, message);
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByOrderCode(string code)
        {
            Console.WriteLine($"--> GETTING order '{code}' ......");

            var message = "";


            var order = await _orderRepo.GetOrderByOrderCode(code);

            if (order == null)
                message = $"Order '{code}' was NOT found !";

            return _resultFact.Result(_mapper.Map<OrderReadDTO>(order), true, message);
        }



        public async Task<IServiceResult<OrderReadDTO>> CreateOrder( int userId, OrderCreateDTO orderCreateDTO)
        {
            var cart = await _cartRepo.GetCartByUserId(userId);

            if (cart == null)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Cart of user '{userId}' does NOT exist !");


            var orderExists = await _orderRepo.ExistsOrderByCartId(cart.CartId);

            if(orderExists)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Order with cart id '{cart.CartId}' already exists !");


            var addressResult = await _httpIdentityService.GetAddressByAddressId(orderCreateDTO.OrderDetails.AddressId);

            if (!addressResult.Status)
                return _resultFact.Result<OrderReadDTO>(null, false, addressResult.Message);


            var message = string.Empty;

            Console.WriteLine($"--> CREATING order for user '{userId}'......");


            var order = _mapper.Map<Order>(orderCreateDTO);

            order.Created = DateTime.Now;

            var orderCodeResult = _orderTools.CreateOrderCode(cart.UserId);

            if (orderCodeResult == null || !orderCodeResult.Status)
                message += Environment.NewLine + $"Failed to generate Order code !";

            order.OrderCode = orderCodeResult.Data ?? "";

            order.CartId = cart.CartId;

            var orderResult = await _orderRepo.CreateOrder(order);

            if (orderResult.State != EntityState.Added || _orderRepo.SaveChanges() < 1)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Order '{order.CartId}': {order.OrderCode} was NOT created !");

            order = (Order)orderResult.Entity;

            var result = _mapper.Map<OrderReadDTO>(order);

            result.OrderDetails.Address = addressResult.Data;

            return _resultFact.Result(result, true, message);
        }


        public async Task<IServiceResult<OrderReadDTO>> UpdateOrder(int userId, OrderUpdateDTO orderUpdateDTO)
        {
            if (orderUpdateDTO.OrderDetails == null)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Order Details data model was NOT provided !");

            var order = await _orderRepo.GetOrderWithDetailsByUserId(userId);

            if (order == null)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Order for user '{userId}' NOT found !");


            var message = "";

            Console.WriteLine($"--> UPDATING order '{order.OrderCode}' ......");


            var addressResult = await _httpIdentityService.GetAddressByAddressId(orderUpdateDTO.OrderDetails.AddressId ?? 0);

            if (addressResult == null || !addressResult.Status)
                message += Environment.NewLine + $"Addresss for order '{userId}' was NOT found ! Reason: '{addressResult.Message}'";

            _mapper.Map(orderUpdateDTO, order);

            if (_orderRepo.SaveChanges() < 1)
                return _resultFact.Result<OrderReadDTO>(null, false, $"No changes were saved into order '{userId}' !");

            var result = _mapper.Map<OrderReadDTO>(order);

            result.OrderDetails.Address = addressResult?.Data;

            return _resultFact.Result(result, true, message);
        }
        


        public async Task<IServiceResult<OrderReadDTO>> CompleteOrder(int userId)
        {
            var order = await _orderRepo.GetOrderWithDetailsByUserId(userId);

            if (order == null)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Order for user '{userId}' NOT found !");


            Console.WriteLine($"--> COMPLETING order '{order.OrderCode}' ......");


            var cartItems = await _cartItemsRepo.GetCartItemsByUserId(userId);

            if(!cartItems.Any())
                return _resultFact.Result<OrderReadDTO>(null, false, $"Can't complete order: '{order.CartId}' for user: '{userId}' because cart is EMPTY !");

            var addressExists = await _httpIdentityService.ExistsAddressByAddressId(order.OrderDetails.AddressId);

            if (addressExists == null)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Failed to acquire address from Identity Service for order: user Id '{userId}', cart Id: '{order.CartId}' !");

            if (!addressExists.Status || !addressExists.Data)
                return _resultFact.Result<OrderReadDTO>(null, false, $"No address found for order: user Id '{userId}', cart Id: '{order.CartId}' ! Reason: '{addressExists.Message}'");

            var activeCartToDelete = await _cartRepo.GetActiveCart(order.CartId);

            if(activeCartToDelete == null)
                return _resultFact.Result<OrderReadDTO>(null, false, $"No active order cart found for '{userId}' !");

            var archiveOrder = await _cartRepo.DeleteActiveCart(activeCartToDelete);

            if (archiveOrder.State != EntityState.Deleted || _orderRepo.SaveChanges() < 1)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Failed to archive Order '{userId}' !");


            //................................................ PAYMENT: ................................................................

            var makePayment = await _httpPaymentService.MakePayment(_mapper.Map<OrderPaymentCreateDTO>(order));

            if (!makePayment.Status)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Failed to make payment for order: user Id '{userId}', cart Id: '{order.CartId}' ! Reason: '{makePayment.Message}'");


            // .................................................................................................. To Do: Send Email .....................................................


            return _resultFact.Result(_mapper.Map<OrderReadDTO>(order), true);
        }




        public async Task<IServiceResult<OrderReadDTO>> DeleteOrder(int userId)
        {
            var order = await _orderRepo.GetOrderByUserId(userId);

            if (order == null)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Order for user '{userId}' NOT found !");


            Console.WriteLine($"--> DELETING order for user '{userId}'......");


            var result = await _orderRepo.DeleteOrder(order);

            order = (Order)result.Entity;

            if (result.State != EntityState.Deleted || _orderRepo.SaveChanges() < 1)
                return _resultFact.Result<OrderReadDTO>(null, false, $"Order of user '{userId}' was NOT deleted !");

            return _resultFact.Result(_mapper.Map<OrderReadDTO>(order), true);
        }




    }
}
