using FluentValidation;
using FinanceTracker.Application.Features.Users.Commands.DesactivateMyAccount;

namespace FinanceTracker.Application.Features.Users.Validators
{
    public class DeactivateMyAccountValidator : AbstractValidator<DeactivateMyAccountCommand>
    {
        public DeactivateMyAccountValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}