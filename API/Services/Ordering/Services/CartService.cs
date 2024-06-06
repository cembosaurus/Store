using AutoMapper;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Microsoft.EntityFrameworkCore;
using Ordering.Data.Repositories.Interfaces;
using Ordering.OrderingBusinessLogic.Interfaces;
using Ordering.Services.Interfaces;
using Services.Ordering.Models;



namespace Ordering.Services
{
    public class CartService : ICartService
    {

        private readonly ICartRepository _cartRepo;
        private readonly IHttpItemService _httpItemService;
        private readonly IHttpItemPriceService _httpItemPriceService;
        private readonly ICartItemService _cartItemsService;
        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;
        private readonly ICartBusinessLogic _cartBusinessLogic;

        public CartService(ICartRepository cartRepo, IServiceResultFactory resultFact, IMapper mapper, ICartBusinessLogic cartBusinessLogic, IHttpItemService httpItemService, IHttpItemPriceService httpItemPriceService, ICartItemService cartItemsService)
        {
            _cartRepo = cartRepo;
            _httpItemService = httpItemService;
            _httpItemPriceService = httpItemPriceService;
            _cartItemsService = cartItemsService;
            _resultFact = resultFact;
            _mapper = mapper;
            _cartBusinessLogic = cartBusinessLogic;
        }




        public async Task<IServiceResult<IEnumerable<CartReadDTO>>> GetCards()
        {
            Console.WriteLine($"--> GETTING carts ......");

            var message = "";


            var carts = await _cartRepo.GetCards();

            if (!carts.Any())
                message = "NO carts were found !";

            return _resultFact.Result(_mapper.Map<IEnumerable<CartReadDTO>>(carts), true, message);
        }



        public async Task<IServiceResult<CartReadDTO>> GetCartByUserId(int userId)
        {
            var message = "";

            Console.WriteLine($"--> GETTING cart for user '{userId}' ......");


            var cart = await _cartRepo.GetCartByUserId(userId);

            if (cart == null)
                return _resultFact.Result<CartReadDTO>(null, true, $"Cart for user '{userId}' was NOT found !");

            var result = _mapper.Map<CartReadDTO>(cart);


            var itemIds = cart.CartItems.Select(i => i.ItemId).ToList();

            var itemsResult = await _httpItemService.GetItems(itemIds);

            if (itemsResult != null || itemsResult.Status)
            {
                //_mapper.Map(items, result.CartItems); // ........... doesn't work properly. Fix It !!!!!!                 IN PROGRESS

                foreach (var ci in result.CartItems)
                {
                    ci.Name = itemsResult.Data.FirstOrDefault(i => i.Id == ci.ItemId).Name;

                    var itemPriceResult = await _httpItemPriceService.GetItemPriceById(ci.ItemId);

                    if (itemPriceResult.Status)
                    {
                        ci.SalePrice = itemPriceResult.Data.SalePrice;
                    }
                    else
                    {
                        message += Environment.NewLine + $"Item price for item '{ci.ItemId}' was NOT found ! Reason: '{itemPriceResult.Message}'";
                    }
                }
            }
            else 
            {
                message += Environment.NewLine + $"No items were found for cart '{cart.CartId}' for user '{cart.UserId}'";
            }

            return _resultFact.Result(result, true);
        }



        public async Task<IServiceResult<CartReadDTO>> CreateCart(int userId)
        {
            if (await _cartRepo.ExistsByUserId(userId))
                return _resultFact.Result<CartReadDTO>(null, false, $"Cart with user Id: '{userId}' already EXISTS !");


            Console.WriteLine($"--> CREATING cart '{userId}'......");


            var cart = new Cart { UserId = userId };

            cart.CartId = Guid.NewGuid();

            cart.ActiveCart = new ActiveCart { UserId = cart.UserId, CartId = cart.CartId };

            var result = await _cartRepo.CreateCart(cart);

            if (result.State != EntityState.Added || _cartRepo.SaveChanges() < 1)
                return _resultFact.Result<CartReadDTO>(null, false, $"Cart for user '{userId}' was NOT created");

            cart = (Cart)result.Entity;

            return _resultFact.Result(_mapper.Map<CartReadDTO>(cart), true);
        }



        public async Task<IServiceResult<CartReadDTO>> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO)
        {
            var cart = await _cartRepo.GetCartByUserId(userId);

            if (cart == null)
                return _resultFact.Result<CartReadDTO>(null, false, $"Cart '{userId}' NOT found !");


            var message = string.Empty;

            Console.WriteLine($"--> UPDATING cart for user '{userId}'......");


            // So far there is no property in Cart entity that can be updated, only CartItems but that can be updated in CartItemsService.
            _mapper.Map(cartUpdateDTO, cart);

            if (_cartRepo.SaveChanges() < 1)
                message += Environment.NewLine + $"No changes in cart '{userId}' were saved.";


            Console.WriteLine($"--> UPDATING cart items for user '{userId}'......");


            if (cartUpdateDTO.Items != null && cartUpdateDTO.Items.Any())
            {
                var itemsUpdate = await _cartItemsService.AddItemsToCart(cart.UserId, cartUpdateDTO.Items);

                if(!string.IsNullOrWhiteSpace(itemsUpdate.Message))
                    message += Environment.NewLine + $"Some Items for cart of user '{cart.UserId}' were not updated ! Reason: {itemsUpdate.Message}";

                if (!itemsUpdate.Status)
                    return _resultFact.Result<CartReadDTO>(null, false, $"Cart '{cart.UserId}' update: 'Success': {message}." + Environment.NewLine + $"Cart '{cart.UserId}' Items update: {itemsUpdate.Message}");
            }

            return _resultFact.Result(_mapper.Map<CartReadDTO>(cart), true, message);
        }



        public async Task<IServiceResult<CartReadDTO>> DeleteCart(int userId)
        {
            var cart = await _cartRepo.GetCartByUserId(userId);

            if(cart == null)
                return _resultFact.Result<CartReadDTO>(null, false, $"Cart '{userId}' NOT found !");


            var message = string.Empty;

            Console.WriteLine($"--> DELETING cart for user '{userId}'......");


            var result = await _cartRepo.DeleteCart(cart);

            if (result.State != EntityState.Deleted || _cartRepo.SaveChanges() <= 0)
                return _resultFact.Result<CartReadDTO>(null, false, $"Cart with id '{userId}' was NOT removed from DB !");

            cart = (Cart)result.Entity;


            Console.WriteLine($"--> ADDING user '{userId}' cart items' amount to stock ......");


            foreach (var ci in cart.CartItems)
            {
                var updateStockAmountResult = await _cartBusinessLogic.AddAmountToStock(ci.ItemId, ci.Amount);

                if (!updateStockAmountResult.Status)
                    message += Environment.NewLine + $"Failed to restore amount '{ci.Amount}' into stock for item '{ci.ItemId}' ! Reason: '{updateStockAmountResult.Message}'";
            }

            return _resultFact.Result(_mapper.Map<CartReadDTO>(cart), true, message);
        }




        public async Task<IServiceResult<bool>> ExistsCartByCartId(Guid cartId)
        {
            var message = "";

            var cartExists = await _cartRepo.ExistsByCartId(cartId);

            if(!cartExists)
                message = $"Cart '{cartId}' does NOT exist !";

            return _resultFact.Result(true, true, message);
        }


    }
}
