

using AutoMapper;
using FinanceTracker.Application.Features.Categories.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryById, CategoryDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryById request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new Exception("Category not found");

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
