using FluentValidation;

namespace Business.Inventory.DTOs.ItemPrice
{
    public class ItemPriceCreateDTO
    {
        public double SalePrice { get; set; }
        public double? RRP { get; set; }
        public int? DiscountPercent { get; set; }
    }

    public class ItemPriceCreateDTO_V : AbstractValidator<ItemPriceCreateDTO>
    {
        public ItemPriceCreateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Item Price create data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x.SalePrice)
                    .GreaterThan(0)
                    .WithMessage("- SalePrice must be greater than 0 !");

                When(x => x.RRP.HasValue, () => {
                    RuleFor(x => x.RRP)
                    .GreaterThan(0)
                    .WithMessage("- RRP must be greater than 0 !");
                });
                When(x => x.DiscountPercent.HasValue, () => {
                    RuleFor(x => x.DiscountPercent)
                    .InclusiveBetween(1, 100)
                    .WithMessage("- Discount Percent must be in range 1 - 100 !");
                });
            });
        }
    }
}
