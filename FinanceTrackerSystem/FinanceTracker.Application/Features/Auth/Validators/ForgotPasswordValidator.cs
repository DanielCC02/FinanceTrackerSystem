using FluentValidation;
using FinanceTracker.Application.Features.Auth.Commands.ForgotPassword;

namespace FinanceTracker.Application.Features.Auth.Validators
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }
    }
}