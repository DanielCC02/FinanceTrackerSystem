using MediatR;


namespace FinanceTracker.Application.Features.Accounts.Commands.DeleteAccount
{
    public record AccountDeleteCommand
    (
        Guid Id
    ) : IRequest<Unit>;
}
