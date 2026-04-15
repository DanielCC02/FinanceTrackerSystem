using FinanceTracker.Application.Features.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Features.Accounts.Commands.DeleteAccount;
using FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount;
using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Application.Features.Accounts.Queries.GetAccountById;
using FinanceTracker.Application.Features.Accounts.Queries.GetAccounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    /// <summary>
    /// Manages user accounts.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new account for the authenticated user.
        /// </summary>
        /// <param name="command">Account creation data</param>
        /// <returns>The created account</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Gets all accounts of the authenticated user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<AccountDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AccountDto>>> GetAll()
        {
            var accounts = await _mediator.Send(new GetAccountsQuery());
            return Ok(accounts);
        }

        /// <summary>
        /// Gets an account by its ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetById(Guid id)
        {
            var account = await _mediator.Send(new GetAccountByIdQuery(id));
            return Ok(account);
        }

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountDto>> Update(Guid id, [FromBody] UpdateAccountCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var updated = await _mediator.Send(command);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes an account (soft delete).
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new AccountDeleteCommand(id));
            return NoContent();
        }

    }
}
