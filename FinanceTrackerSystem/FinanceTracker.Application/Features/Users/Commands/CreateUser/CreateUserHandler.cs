using AutoMapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResponseDto>
    {
        private readonly IApplicationDbContext _dbContext;

        private readonly IPasswordHasher _passwordHasher;

        private readonly IMapper _mapper;

        public CreateUserHandler(IApplicationDbContext dbContext, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<CreateUserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _dbContext.Users
                .AnyAsync(a => a.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                throw new Exception("Email already exists.");
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new User(
                request.Name,
                request.Email,
                passwordHash);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateUserResponseDto>(user);
        }
    }
}
