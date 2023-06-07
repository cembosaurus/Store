using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Archive
{
    public class GetOrderByCartId_Q : IRequest<IServiceResult<OrderReadDTO>>
    {
        public Guid CartId { get; set; }


        public class Validator : AbstractValidator<GetOrderByCartId_Q>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Get Order By Cart Id query model must NOT be NULL !");
                When(x => x != null, () => {
                    RuleFor(x => x.CartId)
                        .NotEqual(Guid.Empty)
                        .WithMessage("- Cart Id must not be empty !");
                });
            }
        }


        public class GetOrderByCartId_H : IRequestHandler<GetOrderByCartId_Q, IServiceResult<OrderReadDTO>>
        {
            private readonly IArchiveService _archiveService;

            public GetOrderByCartId_H(IArchiveService archiveService)
            {
                _archiveService = archiveService;
            }


            public async Task<IServiceResult<OrderReadDTO>> Handle(GetOrderByCartId_Q request, CancellationToken cancellationToken)
            {
                var result = await _archiveService.GetOrderByCartId(request.CartId);

                return result;
            }
        }
    }
}
