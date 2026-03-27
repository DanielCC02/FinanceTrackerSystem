using AutoMapper;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;

        private readonly IMapper _mapper;

        public DeleteUserHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

            if (user.IsDeleted)
                throw new Exception("User already deleted");

            user.IsDeleted = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
