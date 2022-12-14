using FluentValidation;

namespace Business.Ordering.DTOs
{
    public class OrderUpdateDTO
    {
        // Model might be extended in future, hence one nullable property.
        public OrderDetailsUpdateDTO? OrderDetails { get; set; }
    }


    public class OrderUpdateDTO_V : AbstractValidator<OrderUpdateDTO>
    {
        public OrderUpdateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Order Update data model must NOT be NULL !");
            When(x => x != null, () => {
                When(x => x.OrderDetails != null, () => {
                
                    RuleFor(x => x.OrderDetails)
                        .Must(x => !string.IsNullOrWhiteSpace(x!.Name) || x.AddressId.HasValue)
                        .WithMessage("- At least one property is required for Order Details update data model !");
                
                    When(x => !string.IsNullOrWhiteSpace(x.OrderDetails!.Name), () => {
                        RuleFor(x => x.OrderDetails!.Name)
                            .MinimumLength(5)
                            .MaximumLength(30)
                            .WithMessage("- Name length should be between 5 - 30 chartacters !");
                    });
                
                    When(x => x.OrderDetails!.AddressId.HasValue, () => {
                        RuleFor(x => x.OrderDetails!.AddressId)
                            .GreaterThan(0)
                            .WithMessage("- Address Id must be greater than 0 !");
                    });
                });
            });
        }
    }

}
