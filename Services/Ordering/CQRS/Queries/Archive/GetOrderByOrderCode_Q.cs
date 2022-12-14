using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Archive
{
    public class GetOrderByOrderCode_Q : IRequest<IServiceResult<OrderReadDTO>>
    {
        public string Code { get; set; }


        public class Validator : AbstractValidator<GetOrderByOrderCode_Q>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Get Order By Order Code query model must NOT be NULL !");
                When(x => x != null, () => {
                    RuleFor(x => x.Code)
                        .NotEmpty()
                        .WithMessage("- Order Code must NOT be empty !");
                });
            }
        }


        public class GetOrderByOrderCode_H : IRequestHandler<GetOrderByOrderCode_Q, IServiceResult<OrderReadDTO>>
        {
            private readonly IArchiveService _archiveService;

            public GetOrderByOrderCode_H(IArchiveService archiveService)
            {
                _archiveService = archiveService;
            }


            public async Task<IServiceResult<OrderReadDTO>> Handle(GetOrderByOrderCode_Q request, CancellationToken cancellationToken)
            {
                var result = await _archiveService.GetOrderByOrderCode(request.Code);

                return result;
            }
        }
    }
}
