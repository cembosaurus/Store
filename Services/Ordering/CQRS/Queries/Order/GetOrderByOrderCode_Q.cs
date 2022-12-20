using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Order
{
    public class GetOrderByOrderCode_Q : IRequest<IServiceResult<OrderReadDTO>>
    {
        public string OrderCode { get; set; }

        public class Validator : AbstractValidator<GetOrderByOrderCode_Q>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Get Order By Order Code query model must NOT be NULL !");
                When(x => x != null, () => {
                    RuleFor(x => x.OrderCode)
                        .NotEmpty()
                        .WithMessage("- Order code must not be empty !");
                });

            }
        }


        public class GetOrderByOrderCode_H : IRequestHandler<GetOrderByOrderCode_Q, IServiceResult<OrderReadDTO>>
        {
            private readonly IOrderService _orderService;

            public GetOrderByOrderCode_H(IOrderService orderService)
            {
                _orderService = orderService;
            }


            public async Task<IServiceResult<OrderReadDTO>> Handle(GetOrderByOrderCode_Q request, CancellationToken cancellationToken)
            {
                var result = await _orderService.GetOrderByOrderCode(request.OrderCode);

                return result;
            }
        }

    }
}
