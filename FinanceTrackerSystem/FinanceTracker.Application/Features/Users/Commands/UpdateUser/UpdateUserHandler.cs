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
        private readonly ICurrentUserService _currentUser;

        public UpdateUserHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != "Admin" && _currentUser.UserId != request.Id)
                throw new UnauthorizedAccessException("Access denied");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id && (_currentUser.Role == "Admin" || u.Id == _currentUser.UserId), cancellationToken)
                ?? throw new KeyNotFoundException("User not found");

            var normalizedEmail = request.Email.Trim().ToLower();

            var emailExists = await _dbContext.Users
                .AnyAsync(u => u.Email == normalizedEmail && u.Id != request.Id, cancellationToken);

            if (emailExists)
                throw new Exception("Email is already in use");

            user.UpdateProfile(request.FirstName, request.LastName, normalizedEmail);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
