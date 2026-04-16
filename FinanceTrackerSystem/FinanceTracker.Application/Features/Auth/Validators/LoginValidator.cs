using FluentValidation;
using FinanceTracker.Application.Features.Auth.Commands.Login;

namespace FinanceTracker.Application.Features.Auth.Validators
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(email => email == email.Trim())
                .WithMessage("Email cannot contain spaces");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}