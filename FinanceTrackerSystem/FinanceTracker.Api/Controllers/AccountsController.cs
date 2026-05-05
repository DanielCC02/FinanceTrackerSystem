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
    /// Manages bank accounts for the authenticated user.
    /// </summary>
    [ApiController]
    [Route("api/v1/accounts")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // =========================
        // CREATE
        // =========================

        /// <summary>
        /// Creates a new account for the authenticated user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // =========================
        // GET ALL
        // =========================

        /// <summary>
        /// Gets all accounts of the authenticated user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<AccountDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AccountDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAccountsQuery());
            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================

        /// <summary>
        /// Gets an account by its ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAccountByIdQuery(id));
            return Ok(result);
        }

        // =========================
        // UPDATE
        // =========================

        /// <summary>
        /// Updates an existing account of the authenticated user.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> Update(Guid id, [FromBody] UpdateAccountCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "ID mismatch" });

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // =========================
        // DELETE
        // =========================

        /// <summary>
        /// Soft deletes an account and all its transactions.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new AccountDeleteCommand(id));
            return NoContent();
        }
    }
}