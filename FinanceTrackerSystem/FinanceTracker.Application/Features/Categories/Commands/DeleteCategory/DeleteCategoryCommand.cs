using MediatR;

namespace FinanceTracker.Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand
    (
        Guid Id
    ) : IRequest<Unit>;
}
