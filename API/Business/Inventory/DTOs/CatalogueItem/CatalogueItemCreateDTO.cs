using Business.Inventory.DTOs.ItemPrice;
using FluentValidation;

namespace Business.Inventory.DTOs.CatalogueItem
{
    public class CatalogueItemCreateDTO
    {
        public string? Description { get; set; }
        public ItemPriceCreateDTO ItemPrice { get; set; }
        public int? Instock { get; set; }
    }


    public class CatalogueItemCreateDTO_V : AbstractValidator<CatalogueItemCreateDTO>
    {
        public CatalogueItemCreateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Catalogue Item create model must NOT be NULL !");
            When(x => x != null, () => {
                When(x => !string.IsNullOrWhiteSpace(x.Description), () => {
                    RuleFor(x => x.Description)
                    .MaximumLength(100)
                    .WithMessage("- Description should NOT be longer than 100 characters !");
                });

                RuleFor(x => x.ItemPrice)
                    .NotNull()
                    .WithMessage("- Item Price update model must NOT be NULL !");
                When(x => x.ItemPrice != null, () =>
                {
                    RuleFor(x => x.ItemPrice.SalePrice)
                        .GreaterThan(0)
                        .WithMessage("- SalePrice must be greater than 0 !");
                    When(x => x.ItemPrice.RRP.HasValue, () => {
                        RuleFor(x => x.ItemPrice.RRP)
                        .GreaterThan(0)
                        .WithMessage("- RRP must be greater than 0 !");
                    });
                    When(x => x.ItemPrice.DiscountPercent.HasValue, () => {
                        RuleFor(x => x.ItemPrice.DiscountPercent)
                        .InclusiveBetween(1, 100)
                        .WithMessage("- Discount Percent must be in range 1 - 100 !");
                    });
                });

                When(x => x.Instock != null, () => {
                    RuleFor(x => x.Instock)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("- Instock amount must be greater or equal to 0 !");
                });
            });
        }
    }
}