﻿using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;

namespace API_Gateway.HttpServices.Ordering.Interfaces
{
    public interface IHttpCartItemService
    {
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> AddItemsToCart(int cartId, IEnumerable<CartItemUpdateDTO> itemsToAdd);
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> DeleteCartItems(int userId, IEnumerable<int> itemIds);
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetAllCardItems();
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetCartItems(int userId);
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> RemoveCartItems(int cartId, IEnumerable<CartItemUpdateDTO> itemsToRemove);
    }
}
