using FluentValidation;
using FinanceTracker.Application.Features.Accounts.Commands.CreateAccount;

namespace FinanceTracker.Application.Features.Accounts.Validators
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Account name is required")
                .MaximumLength(50).WithMessage("Account name cannot exceed 50 characters");


            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid account type");
        }
    }
}