using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Archive
{
    public class GetAllOrders_Q : IRequest<IServiceResult<IEnumerable<OrderReadDTO>>>
    {


        public class GetAllOrders_H : IRequestHandler<GetAllOrders_Q, IServiceResult<IEnumerable<OrderReadDTO>>>
        {
            private readonly IArchiveService _archiveService;

            public GetAllOrders_H(IArchiveService archiveService)
            {
                _archiveService = archiveService;
            }


            public async Task<IServiceResult<IEnumerable<OrderReadDTO>>> Handle(GetAllOrders_Q request, CancellationToken cancellationToken)
            {
                var result = await _archiveService.GetAllOrders();

                return result;
            }
        }
    }
}
