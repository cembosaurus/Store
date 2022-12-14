using Business.Inventory.DTOs.ItemPrice;
using FluentValidation;

namespace Business.Inventory.DTOs.CatalogueItem
{
    public class CatalogueItemUpdateDTO
    {
        public string? Description { get; set; }
        public ItemPriceUpdateDTO? ItemPrice { get; set; }
        public int? Instock { get; set; }
    }


    public class CatalogueItemUpdateDTO_V : AbstractValidator<CatalogueItemUpdateDTO>
    {
        public CatalogueItemUpdateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Catalogue Item update data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x)
                    .Must(x => x.Description != null || x.ItemPrice != null || x.Instock != null)
                    .WithMessage("- At least one property is required in Catalogue Item update model !");

                When(x => !string.IsNullOrWhiteSpace(x.Description), () => {
                    RuleFor(x => x.Description)
                        .MaximumLength(100)
                        .WithMessage("- Description should NOT be longer than 100 chartacters !");
                });

                When(x => x.ItemPrice != null, () => {
                    RuleFor(x => x)
                        .Must(x => x.ItemPrice!.SalePrice.HasValue || x.ItemPrice.RRP.HasValue || x.ItemPrice.DiscountPercent.HasValue)
                        .WithMessage("- At least one property is required for Item Price update !");

                    When(x => x.ItemPrice!.SalePrice.HasValue, () => {
                        RuleFor(x => x.ItemPrice!.SalePrice)
                            .GreaterThan(0)
                            .WithMessage("- Sales Price should be HIGHER than '0' !");
                    });

                    When(x => x.ItemPrice!.RRP.HasValue, () => {
                        RuleFor(x => x.ItemPrice!.RRP)
                            .GreaterThan(0)
                            .WithMessage("- RRP should be HIGHER than '0' !");
                    });

                    When(x => x.ItemPrice!.DiscountPercent.HasValue, () => {
                        RuleFor(x => x.ItemPrice!.DiscountPercent)
                            .InclusiveBetween(0, 100)
                            .WithMessage("- Discount percent should be between '0' and '100' !");
                    });
                });

                When(x => x.Instock.HasValue, () => {
                    RuleFor(x => x.Instock)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("- Instock amount must be greater or equal to 0 !");
                });
            });
        }
    }
}
