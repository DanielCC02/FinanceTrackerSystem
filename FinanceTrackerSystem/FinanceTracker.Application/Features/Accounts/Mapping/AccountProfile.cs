using AutoMapper;
using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Features.Accounts.Mapping
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountDto>();
        }
    }
}
