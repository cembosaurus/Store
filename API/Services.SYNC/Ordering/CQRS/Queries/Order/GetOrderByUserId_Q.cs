using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Order
{
    public class GetOrderByUserId_Q : IRequest<IServiceResult<OrderReadDTO>>
    {
        public int? UserId { get; set; }


        public class Validator : AbstractValidator<GetOrderByUserId_Q>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Get Order By User Id query model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId != null, () => {
                        RuleFor(x => x.UserId)
                        .GreaterThan(0)
                        .WithMessage("- User Id must be greater than 0 !");
                    });
                });

            }
        }


        public class GetOrderByUserId_H : IRequestHandler<GetOrderByUserId_Q, IServiceResult<OrderReadDTO>>
        {
            private readonly IOrderService _orderService;

            public GetOrderByUserId_H(IOrderService orderService)
            {
                _orderService = orderService;
            }


            public async Task<IServiceResult<OrderReadDTO>> Handle(GetOrderByUserId_Q request, CancellationToken cancellationToken)
            {
                var result = await _orderService.GetOrderByUserId(request.UserId ?? 0);

                return result;
            }
        }
    }
}
