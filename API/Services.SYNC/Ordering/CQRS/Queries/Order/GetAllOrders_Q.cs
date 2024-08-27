using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Order
{
    public class GetAllOrders_Q : IRequest<IServiceResult<IEnumerable<OrderReadDTO>>>
    {


        public class GetAllOrders_H : IRequestHandler<GetAllOrders_Q, IServiceResult<IEnumerable<OrderReadDTO>>>
        {
            private readonly IOrderService _orderService;

            public GetAllOrders_H(IOrderService orderService)
            {
                _orderService = orderService;
            }


            public async Task<IServiceResult<IEnumerable<OrderReadDTO>>> Handle(GetAllOrders_Q request, CancellationToken cancellationToken)
            {
                var result = await _orderService.GetAllOrders();

                return result;
            }
        }
    }
}
