using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Identity.Http.Clients.Interfaces
{
    public interface IHttpUserClient
    {
        Task<HttpResponseMessage> EditUserRoles(int id, IEnumerable<string> roles);
        Task<HttpResponseMessage> GetAllUsers();
        Task<HttpResponseMessage> GetAllUsersWithRoles();
        Task<HttpResponseMessage> GetCurrentUser();
        Task<HttpResponseMessage> GetUserById(int id);
        Task<HttpResponseMessage> GetUserByName(string name);
        Task<HttpResponseMessage> GetUserWithRoles(int id);
    }
}
