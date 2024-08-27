using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Order
{
    public class CreateOrder_C : IRequest<IServiceResult<OrderReadDTO>>
    {
        public int? UserId { get; set; }
        public OrderCreateDTO OrderCreateDTO { get; set; }


        public class Validator : AbstractValidator<CreateOrder_C>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Create Order command model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId == null, () =>
                    {
                        RuleFor(x => x.OrderCreateDTO)
                            .NotNull()
                            .WithMessage("- Order Ceate data model must NOT be NULL !");

                        When(x => x.OrderCreateDTO != null, () => {

                            RuleFor(x => x.OrderCreateDTO)
                            .ChildRules(x => {
                                RuleFor(x => x.OrderCreateDTO.OrderDetails)
                                    .NotNull()
                                    .WithMessage("- Order Details must NOT be NULL !");

                                When(x => x.OrderCreateDTO.OrderDetails != null, () => {
                                    RuleFor(x => x.OrderCreateDTO.OrderDetails)
                                    .ChildRules(x =>
                                    {
                                        x.RuleFor(x => x.Name)
                                        .NotNull()
                                        .WithMessage("- Name must NOT be NULL !");
                                        When(x => string.IsNullOrWhiteSpace(x.OrderCreateDTO.OrderDetails.Name), () => {
                                            RuleFor(x => x.OrderCreateDTO.OrderDetails.Name)
                                                .MinimumLength(5)
                                                .MaximumLength(30)
                                                .WithMessage("- Name length should be between 5 - 30 chartacters !");
                                        });
                                        x.RuleFor(x => x.AddressId)
                                            .GreaterThan(0)
                                            .WithMessage("- Address Id is NOT in the range !");
                                    });
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


        public class CreateOrder_H : IRequestHandler<CreateOrder_C, IServiceResult<OrderReadDTO>>
        {
            private readonly IOrderService _orderService;

            public CreateOrder_H(IOrderService orderService)
            {
                _orderService = orderService;
            }


            public async Task<IServiceResult<OrderReadDTO>> Handle(CreateOrder_C request, CancellationToken cancellationToken)
            {
                var result = await _orderService.CreateOrder(request.UserId ?? 0, request.OrderCreateDTO);

                return result;
            }
        }

    }
}
