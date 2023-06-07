using FluentValidation;

namespace Business.Inventory.DTOs.ItemPrice
{
    public class ItemPriceUpdateDTO
    {
        public double? SalePrice { get; set; }
        public double? RRP { get; set; }
        public int? DiscountPercent { get; set; }
    }


    public class Validation : AbstractValidator<ItemPriceUpdateDTO>
    {
        public Validation()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Item Price update model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x)
                    .Must(x => x.SalePrice.HasValue || x.RRP.HasValue || x.DiscountPercent.HasValue)
                    .WithMessage("- At least one property is required for Item Price update !");

                When(x => x.SalePrice.HasValue, () => {
                    RuleFor(x => x.SalePrice)
                        .GreaterThan(0)
                        .WithMessage("- Sales Price should be HIGHER than '0' !");
                });

                When(x => x.RRP.HasValue, () => {
                    RuleFor(x => x.RRP)
                        .GreaterThan(0)
                        .WithMessage("- RRP should be HIGHER than '0' !");
                });

                When(x => x.DiscountPercent.HasValue, () => {
                    RuleFor(x => x.DiscountPercent)
                        .InclusiveBetween(0, 100)
                        .WithMessage("- Discount percent should be between '0' and '100' !");
                });
            });
        }
    }
}
