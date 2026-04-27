using FluentValidation;
using FinanceTracker.Application.Features.Transactions.Commands.DeleteTransaction;

namespace FinanceTracker.Application.Features.Transactions.Validators
{
    public class DeleteTransactionValidator : AbstractValidator<DeleteTransactionCommand>
    {
        public DeleteTransactionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Transaction ID is required");
        }
    }
}