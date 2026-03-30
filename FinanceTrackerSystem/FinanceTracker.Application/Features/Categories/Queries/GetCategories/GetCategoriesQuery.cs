using FinanceTracker.Application.Features.Categories.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery
    () : IRequest<List<CategoryDto>>;
}
