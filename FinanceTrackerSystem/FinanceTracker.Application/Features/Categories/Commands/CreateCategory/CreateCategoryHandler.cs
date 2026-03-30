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

        public CreateCategoryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _dbContext.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                throw new KeyNotFoundException("User not found.");

            var category = new Category(
                request.UserId,
                request.Name,
                request.Type);

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
