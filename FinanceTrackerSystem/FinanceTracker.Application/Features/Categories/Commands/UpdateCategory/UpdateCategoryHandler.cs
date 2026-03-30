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

        public UpdateCategoryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Category not found");

            category.Update(request.Name, request.Type);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
