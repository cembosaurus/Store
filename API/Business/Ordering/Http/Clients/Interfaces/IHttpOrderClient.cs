using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using System.Threading.Tasks;

namespace Business.Ordering.Http.Clients.Interfaces
{
    public interface IHttpOrderClient
    {
        Task<HttpResponseMessage> CompleteOrder(int userId);
        Task<HttpResponseMessage> CreateOrder(int userId, OrderCreateDTO orderDTO);
        Task<HttpResponseMessage> DeleteOrder(int userId);
        Task<HttpResponseMessage> GetAllOrders();
        Task<HttpResponseMessage> GetOrderByCartId(Guid id);
        Task<HttpResponseMessage> GetOrderByOrderCode(string code);
        Task<HttpResponseMessage> GetOrderByUserId(int id);
        Task<HttpResponseMessage> UpdateOrder(int userId, OrderUpdateDTO orderDTO);
    }
}
