using AutoMapper;
using FinanceTracker.Application.Features.Categories.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinanceTracker.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateCategoryHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim().ToLower();

            var exists = await _dbContext.Categories
                .AnyAsync(c =>
                    c.Name == name &&
                    (c.UserId == _currentUser.UserId || c.UserId == null),
                    cancellationToken);

            if (exists)
                throw new InvalidOperationException("Category already exists");

            var category = new Category(
                _currentUser.UserId,
                name,
                request.SuggestedType,
                request.Icon
            );

            _dbContext.Categories.Add(category);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
