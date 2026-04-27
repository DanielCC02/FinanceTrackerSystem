using AutoMapper;
using FinanceTracker.Application.Features.Categories.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinanceTracker.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UpdateCategoryHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim().ToLower();

            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c =>
                    c.Id == request.Id &&
                    c.UserId == _currentUser.UserId,
                    cancellationToken)
                ?? throw new KeyNotFoundException("Category not found");

            if (category.Name != name)
            {
                var exists = await _dbContext.Categories
                .AnyAsync(c =>
                    c.Id != request.Id &&
                    c.Name == name &&
                    (c.UserId == _currentUser.UserId || c.UserId == null),
                    cancellationToken);

                if (exists)
                    throw new InvalidOperationException("Category already exists");
            }

            category.Update(name, request.Icon, request.SuggestedType);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
