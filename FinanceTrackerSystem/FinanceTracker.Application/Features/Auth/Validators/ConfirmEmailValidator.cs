using FluentValidation;
using FinanceTracker.Application.Features.Auth.Commands.ConfirmEmail;

namespace FinanceTracker.Application.Features.Auth.Validators
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");
        }
    }
}