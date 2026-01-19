using AutoMapper;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using Microsoft.EntityFrameworkCore;
using Scheduler.Data.Repositories.Interfaces;
using Scheduler.Models;
using Scheduler.Services.Interfaces;



namespace Scheduler.Services
{
    public class OrderingService : IArchiveService, ICartItemsService, ICartService, IOrderService
    {
        private readonly ICartItemLockRepository _cartItemLockRepo;
        private readonly IHttpCatalogueItemService _httpCatalogueItemService;
        private readonly IHttpCartService _httpCartService;
        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;
        private readonly int _lockedForDays;

        public OrderingService(IConfiguration config, IServiceResultFactory resultFact, IHttpCatalogueItemService httpCatalogueItemService, IHttpCartService httpCartService, ICartItemLockRepository cartItemLockRepo, 
            IMapper mapper)
        {
            _cartItemLockRepo = cartItemLockRepo;
            _httpCatalogueItemService = httpCatalogueItemService;
            _httpCartService = httpCartService;
            _resultFact = resultFact;
            _mapper = mapper;
            int.TryParse(config.GetSection("Config:Local:ItemsLock:ItemLockedForDays").Value, out _lockedForDays);
        }






        public async Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> GetAllLocks()
        {
            Console.WriteLine("---> GETTING all locks ....");

            var message = "";


            var locksResult = await _cartItemLockRepo.GetAllCartItemLocks();

            if (locksResult == null || !locksResult.Any())
                message = "No locks were found.";

            var result = _mapper.Map<IEnumerable<CartItemsLockReadDTO>>(locksResult);

            return _resultFact.Result(result, true, message);
        }




        public async Task<IServiceResult<CartItemsLockReadDTO>> CartItemsLock(CartItemsLockCreateDTO cartItemsToLockDTO)
        {
            var cartExists = await _httpCartService.ExistsCartByCartId(cartItemsToLockDTO.CartId);

            if(!cartExists.Status || !cartExists.Data)
                return _resultFact.Result<CartItemsLockReadDTO>(null, false, $"Cart '{cartItemsToLockDTO.CartId}' does NOT exist !");


            var message = "";

            Console.WriteLine($"--> LOCKING items on cart '{cartItemsToLockDTO.CartId}' for '{_lockedForDays}' days ......");


            var cartItemsIdsToLock = new List<CartItemLock>();
            var lockNow = DateTime.Now;

            foreach (var i in cartItemsToLockDTO.ItemsIds)
            {
                var itemExists = await _httpCatalogueItemService.ExistsCatalogueItemById(i);

                if (!itemExists.Status)
                {
                    message += Environment.NewLine + $"Cart item '{i}' does NOT exist !";
                }
                else
                {
                    var cartItemLockInDB = await _cartItemLockRepo.GetCartItemLock(cartItemsToLockDTO.CartId, i);

                    if (cartItemLockInDB == null)
                    {
                        // New CartItemLock:

                        var cartItemToLock = new CartItemLock
                        {
                            CartId = cartItemsToLockDTO.CartId,
                            ItemId = i,
                            LockedForDays = _lockedForDays,
                            Locked = lockNow
                        };


                        // Create:

                        await _cartItemLockRepo.CreateCartItemLock(cartItemToLock);

                        if (_cartItemLockRepo.SaveChanges() < 1)
                            message += Environment.NewLine + $"Failed to lock item '{cartItemToLock.ItemId}' on cart '{cartItemToLock.CartId}' in repository !";

                        cartItemsIdsToLock.Add(cartItemToLock);
                    }
                    // Temporarely commented out:
                    // ... if CartItem is already locked by previous/first insert then there is no need to do anything:
                    //
                    //else
                    //{
                    //    // Update:

                    //    // - Expire date is already set by first locked item of the same ID,
                    //    //  no need for extending it (could be abused by periodicaly incrementing amount on cart and holding it in there,
                    //    //  making it unaccessible for other customers).
                    //    // - Amount of locked items is provided by Cart Service

                    //    //cartItemLockInDB.Locked = lockNow;
                    //    //cartItemLockInDB.LockedForDays = _lockedForDays;

                    //    if (_cartItemLockRepo.SaveChanges() < 1)
                    //        message += Environment.NewLine + $"Failed to update locked item '{cartItemLockInDB.ItemId}' on cart '{cartItemLockInDB.CartId}' in repository !";
                    //}                   
                }
            }

            var result = new CartItemsLockReadDTO { 
                CartId = cartItemsToLockDTO.CartId, 
                ItemsIds = cartItemsIdsToLock.Select(cil => cil.ItemId).ToList(), 
                LockedForDays = _lockedForDays, 
                Locked = lockNow
            };

            return _resultFact.Result(result, true);
        }



        public async Task<IServiceResult<CartItemsLockReadDTO>> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLockDTO)
        {
            var cartExists = await _httpCartService.ExistsCartByCartId(cartItemsToUnLockDTO.CartId);

            if (!cartExists.Status || !cartExists.Data)
                return _resultFact.Result<CartItemsLockReadDTO>(null, false, $"Cart '{cartItemsToUnLockDTO.CartId}' does NOT exist !");


            var message = "";

            Console.WriteLine($"--> UNLOCKING items from cart '{cartItemsToUnLockDTO.CartId}' for '{_lockedForDays}' days ......");



            foreach (var i in cartItemsToUnLockDTO.ItemsIds)
            {
                var cartItemLockInDB = await _cartItemLockRepo.GetCartItemLock(cartItemsToUnLockDTO.CartId, i);

                if (cartItemLockInDB == null)
                {
                    message += Environment.NewLine + $"Cart item '{i}' to unlock does NOT exist in scheduler !";

                    continue;
                }

                var cartItemUnlockResult = await _cartItemLockRepo.DeleteCartItemLock(cartItemLockInDB);

                if (cartItemUnlockResult.State != EntityState.Deleted || _cartItemLockRepo.SaveChanges() < 1)
                    message += Environment.NewLine + $"Cart item '{i}' was NOT unlocked !";
            }

            var result = new CartItemsLockReadDTO
            {
                CartId = cartItemsToUnLockDTO.CartId,
                ItemsIds = cartItemsToUnLockDTO.ItemsIds,
                LockedForDays = _lockedForDays
            };

            return _resultFact.Result(result, true);
        }




        public async Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks)
        {
            if (cartItemLocks == null || !cartItemLocks.Any())
                return _resultFact.Result<IEnumerable<CartItemsLockReadDTO>>(null, false, "Cart-item-unlock data models were NOT provided !");

            var message = "";


            // HTTP Request --> DELETE expired item locks from Cart API service:

            var removeCartItemResult = await _httpCartService.RemoveExpiredItemsFromCart(cartItemLocks);

            if (!removeCartItemResult.Status)
                return _resultFact.Result<IEnumerable<CartItemsLockReadDTO>>(null, false, removeCartItemResult.Message);

            foreach (var cil in cartItemLocks)
            {
                foreach (var i in cil.ItemsIds)
                {

                    var lockToDelete = await _cartItemLockRepo.GetCartItemLock(cil.CartId, i);

                    if (lockToDelete == null)
                        message += Environment.NewLine + $"Lock for cart: '{cil.CartId}' and item: '{i}' does NOT exist !";

                    var removeCartItemLockResult = await _cartItemLockRepo.DeleteCartItemLock(lockToDelete);

                    if (removeCartItemLockResult.State != EntityState.Deleted || _cartItemLockRepo.SaveChanges() < 1)
                        message += Environment.NewLine + $"Lock for cart: '{cil.CartId}' and item: '{i}' was NOT deleted !";
                }
            }

            return _resultFact.Result(removeCartItemResult.Data, true, removeCartItemResult.Message);
        }



    }
}
