using FluentValidation;
using FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount;

namespace FinanceTracker.Application.Features.Accounts.Validators
{
    public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Account ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Account name is required")
                .MaximumLength(100)
                .MaximumLength(50).WithMessage("Account name cannot exceed 50 characters");
                

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid account type");
        }
    }
}