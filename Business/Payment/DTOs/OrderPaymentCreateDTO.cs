using FluentValidation;

namespace Business.Payment.DTOs
{
    public class OrderPaymentCreateDTO
    {
        public Guid CartId { get; set; }
        public string OrderCode { get; set; }
    }


    public class OrderPaymentCreateDTO_V : AbstractValidator<OrderPaymentCreateDTO>
    {
        public OrderPaymentCreateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Order Payment create data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x.CartId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("- Cart Id must not be empty !");

                RuleFor(x => x.OrderCode)
                    .NotNull()
                    .WithMessage("- Order Code must NOT be NULL !");
                When(x => !string.IsNullOrWhiteSpace(x.OrderCode), () => {
                    RuleFor(x => x.OrderCode)
                        .NotEmpty()
                        .WithMessage("- Order Code must NOT be empty !")
                        .MinimumLength(20)
                        .MaximumLength(30)
                        .WithMessage("- Order Code length should be between 20 - 30 chartacters !");
                });
            });
        }
    }
}
