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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IApplicationDbContext dbContext, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found");
            
            if (user.IsDeleted)
                throw new Exception("User is deleted");

            var emailExists = await _dbContext.Users
                .AnyAsync(a => a.Email == request.email && a.Id != request.Id, cancellationToken);

            if(emailExists)
                throw new Exception("Email already exists");

            user.GetType().GetProperty("Name")!.SetValue(user, request.name);
            user.GetType().GetProperty("Email")!.SetValue(user, request.email);

            if (!string.IsNullOrWhiteSpace(request.password))
            {
                var passwordHash = _passwordHasher.HashPassword(request.password);
                user.GetType().GetProperty("PasswordHash")!.SetValue(user, passwordHash);

            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
