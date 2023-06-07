﻿using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.HttpServices.Identity.Interfaces
{
    public interface IHttpUserService
    {
        Task<IServiceResult<IEnumerable<string>>> EditUserRoles(int id, IEnumerable<string> roles);
        Task<IServiceResult<IEnumerable<UserReadDTO>>> GetAllUsers();
        Task<IServiceResult<IEnumerable<UserWithRolesReadDTO>>> GetAllUsersWithRoles();
        Task<IServiceResult<UserReadDTO>> GetCurrentUser();
        Task<IServiceResult<UserReadDTO>> GetUserById(int id);
        Task<IServiceResult<UserReadDTO>> GetUserByName(string name);
        Task<IServiceResult<UserWithRolesReadDTO>> GetUserWithRoles(int id);
    }
}
