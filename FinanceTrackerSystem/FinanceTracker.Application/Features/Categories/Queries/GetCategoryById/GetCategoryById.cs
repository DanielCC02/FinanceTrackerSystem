using FinanceTracker.Application.Features.Categories.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryById
    (
        Guid Id
    ) : IRequest<CategoryDto>;
}
