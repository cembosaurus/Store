using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Order
{
    public class UpdateOrder_C : IRequest<IServiceResult<OrderReadDTO>>
    {
        public int? UserId { get; set; }
        public OrderUpdateDTO OrderUpdateDTO { get; set; }


        public class Validator : AbstractValidator<UpdateOrder_C>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Update Order command model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId == null, () =>
                    {
                        RuleFor(x => x.OrderUpdateDTO)
                        .NotNull()
                        .WithMessage("- Order Update model should NOT be NULL !");

                        // If OrderDetails is only property in model then
                        // test it for NULL to prevent passing empty OrderUpdate model.
                        // If there are more than one property then test them for 'at least one not null or empty'
                        RuleFor(x => x.OrderUpdateDTO.OrderDetails)
                        .NotNull()
                        .WithMessage("- Order Details must NOT be NULL !");

                        When(x => x.OrderUpdateDTO != null, () => {

                            When(x => x.OrderUpdateDTO.OrderDetails != null, () => {

                                RuleFor(x => x.OrderUpdateDTO.OrderDetails)
                                .Custom((x, context) =>
                                {
                                    if ((x!.Name == null || string.IsNullOrEmpty(x.Name)) && (x.AddressId == null || x.AddressId < 1))
                                        context.AddFailure("- Address Id OR Name must be provided !");
                                });
                            });
                        });
                    });
                    When(x => x.UserId != null, () => {
                        RuleFor(x => x.UserId)
                        .GreaterThan(0)
                        .WithMessage("- User Id must be greater than 0 !");
                    });
                });
            }
        }


        public class UpdateOrder_H : IRequestHandler<UpdateOrder_C, IServiceResult<OrderReadDTO>>
        {
            private readonly IOrderService _orderService;

            public UpdateOrder_H(IOrderService orderService)
            {
                _orderService = orderService;
            }


            public async Task<IServiceResult<OrderReadDTO>> Handle(UpdateOrder_C request, CancellationToken cancellationToken)
            {
                var result = await _orderService.UpdateOrder(request.UserId ?? 0, request.OrderUpdateDTO);

                return result;
            }
        }

    }
}
