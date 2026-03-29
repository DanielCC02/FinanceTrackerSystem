using FinanceTracker.Application.Features.Categories.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery
    () : IRequest<List<CategoryDto>>;
}
