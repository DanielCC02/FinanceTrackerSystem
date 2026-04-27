using FluentValidation;
using FinanceTracker.Application.Features.Accounts.Commands.DeleteAccount;

namespace FinanceTracker.Application.Features.Accounts.Validators
{
    public class DeleteAccountValidator : AbstractValidator<AccountDeleteCommand>
    {
        public DeleteAccountValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Account ID is required");
        }
    }
}