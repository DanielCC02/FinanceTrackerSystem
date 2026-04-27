using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceTracker.Application.Features.Categories.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetCategoriesHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Categories
                .Where(c =>
                    c.UserId == _currentUser.UserId ||
                    c.UserId == null);

            if (request.Type.HasValue)
            {
                query = query.Where(c =>
                    c.SuggestedType == null ||
                    c.SuggestedType == request.Type);
            }

            return await query
                .OrderByDescending(c => c.UserId == null) 
                .ThenBy(c => c.Name)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
