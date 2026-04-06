using FinanceTracker.Application.Features.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Features.Accounts.Commands.DeleteAccount;
using FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount;
using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Application.Features.Accounts.Queries.GetAccountById;
using FinanceTracker.Application.Features.Accounts.Queries.GetAccounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountDto>>> GetAll()
        {
            var accounts = await _mediator.Send(new GetAccountsQuery());
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetById(Guid id)
        {
            var account = await _mediator.Send(new GetAccountByIdQuery(id));
            return Ok(account);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDto>> Update(Guid id, UpdateAccountCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var updated = await _mediator.Send(command);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // 🔥 solo admin
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new AccountDeleteCommand(id));
            return NoContent();
        }

    }
}
