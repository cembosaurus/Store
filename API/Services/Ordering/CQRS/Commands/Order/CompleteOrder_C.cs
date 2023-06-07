using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Order
{
    public class CompleteOrder_C : IRequest<IServiceResult<OrderReadDTO>>
    {
        public int? UserId { get; set; }


        public class Validator : AbstractValidator<CompleteOrder_C>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Complete Order command model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId != null, () => {
                        RuleFor(x => x.UserId)
                        .GreaterThan(0)
                        .WithMessage("- User Id must be greater than 0 !");
                    });
                });
            }
        }


        public class CompleteOrder_H : IRequestHandler<CompleteOrder_C, IServiceResult<OrderReadDTO>>
        {
            private readonly IOrderService _orderService;

            public CompleteOrder_H(IOrderService orderService)
            {
                _orderService = orderService;
            }


            public async Task<IServiceResult<OrderReadDTO>> Handle(CompleteOrder_C request, CancellationToken cancellationToken)
            {
                var result = await _orderService.CompleteOrder(request.UserId ?? 0);

                return result;
            }
        }

    }
}
