using FinanceTracker.Application.Features.Accounts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccountById
{
    public record GetAccountByIdQuery
    (
        Guid Id
    ) : IRequest<AccountDto>;
}
