using FluentValidation;

namespace Business.Identity.DTOs
{
    public class UserToLoginDTO
    {
        public string Name { get; set; }
        public string Password { get; set; }

    }


    public class UserToLoginDTO_V : AbstractValidator<UserToLoginDTO>
    {
        public UserToLoginDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- User To Register data model must NOT be NULL ");
            When(x => x != null, () => {
                RuleFor(x => x.Name)
                    .NotNull()
                    .WithMessage("- Name must NOT be NULL !");
                When(x => !string.IsNullOrWhiteSpace(x.Name), () => {
                    RuleFor(x => x.Name)
                        .NotEmpty()
                        .WithMessage("- Name must NOT be empty !")
                        .MinimumLength(2)
                        .MaximumLength(30)
                        .WithMessage("- Name length should be between 2 - 30 chartacters !");
                });

                RuleFor(x => x.Password)
                    .NotNull()
                    .WithMessage("- Password must NOT be NULL !");
                When(x => !string.IsNullOrWhiteSpace(x.Password), () => {
                    RuleFor(x => x.Password)
                        .NotEmpty()
                        .WithMessage("- Password must NOT be empty !")
                        .MinimumLength(4)
                        .MaximumLength(8)
                        .WithMessage("- Password length should be between 4 - 8 chartacters !");
                });
            });
        }
    }

}
