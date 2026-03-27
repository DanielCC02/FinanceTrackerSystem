using AutoMapper;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinanceTracker.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {

        private readonly IApplicationDbContext _dbContext;

        private readonly IMapper _mapper;

        public GetUserByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

            return _mapper.Map<UserDto>(user);
        }
    }
}
