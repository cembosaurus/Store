using Business.Libraries.ServiceResult.Interfaces;



namespace Ordering.Tools.Interfaces
{
    public interface IOrder
    {
        IServiceResult<string> CreateOrderCode(int cartId);
        IServiceResult<string> DecodeOrderCode(string orderId);
    }
}
