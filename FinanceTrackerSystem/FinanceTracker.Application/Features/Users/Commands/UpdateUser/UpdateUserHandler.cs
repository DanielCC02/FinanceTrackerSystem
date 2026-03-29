using AutoMapper;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
                ?? throw new Exception("User not found");

            if (user.IsDeleted)
                throw new Exception("User is deleted");

            var emailExists = await _dbContext.Users
                .AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);

            if (emailExists)
                throw new Exception("Email is already in use");

            user.Update(request.Name, request.Email);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
