using FinanceTracker.Application.Features.Transactions.Queries.GetTransactions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Application.Features.Transactions.Validators
{
    public class GetTransactionsValidator : AbstractValidator<GetTransactionsQuery>
    {
        public GetTransactionsValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Account ID is required");

            RuleFor(x => x.From)
                .LessThanOrEqualTo(x => x.To)
                .When(x => x.From.HasValue && x.To.HasValue)
                .WithMessage("From date must be before To date");
        }
    }
}
