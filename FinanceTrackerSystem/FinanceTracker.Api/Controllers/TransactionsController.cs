using FinanceTracker.API.Requests;
using FinanceTracker.Application.Features.Transactions.Commands.CreateTransaction;
using FinanceTracker.Application.Features.Transactions.Commands.DeleteTransaction;
using FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction;
using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Application.Features.Transactions.Queries.GetAllTransactions;
using FinanceTracker.Application.Features.Transactions.Queries.GetTransactionById;
using FinanceTracker.Application.Features.Transactions.Queries.GetTransactions;
using FinanceTracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // =========================
        // CREATE
        // =========================

        /// <summary>
        /// Creates a new transaction for a specific account.
        /// </summary>
        [HttpPost("/api/v1/accounts/{accountId}/transactions")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Guid accountId, [FromBody] CreateTransactionRequest request)
        {
            var command = new CreateTransactionCommand(
                accountId,
                request.CategoryId,
                request.Amount,
                request.Type,
                request.Description,
                request.Date);

            var result = await _mediator.Send(command);

            return CreatedAtRoute(
                "GetTransactionById",
                new { accountId, id = result.Id },
                result);
        }

        // =========================
        // GET ALL (por cuenta)
        // =========================

        /// <summary>
        /// Gets all transactions for a specific account with optional filters.
        /// </summary>
        [HttpGet("/api/v1/accounts/{accountId}/transactions")]
        [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TransactionDto>>> GetAll(
            Guid accountId,
            [FromQuery] TransactionType? type,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] Guid? categoryId)
        {
            var result = await _mediator.Send(new GetTransactionsQuery(
                accountId,
                type,
                from,
                to,
                categoryId));

            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================

        /// <summary>
        /// Gets a transaction by ID within a specific account.
        /// </summary>
        [HttpGet("/api/v1/accounts/{accountId}/transactions/{id}", Name = "GetTransactionById")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionDto>> GetById(Guid accountId, Guid id)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery(accountId, id));
            return Ok(result);
        }

        // =========================
        // UPDATE
        // =========================

        /// <summary>
        /// Updates a transaction within a specific account.
        /// </summary>
        [HttpPut("/api/v1/accounts/{accountId}/transactions/{id}")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionDto>> Update(
            Guid accountId,
            Guid id,
            [FromBody] UpdateTransactionRequest request)
        {
            var command = new UpdateTransactionCommand(
                accountId,
                id,
                request.CategoryId,
                request.Amount,
                request.Type,
                request.Description,
                request.Date);

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // =========================
        // DELETE
        // =========================

        /// <summary>
        /// Deletes a transaction within a specific account.
        /// </summary>
        [HttpDelete("/api/v1/accounts/{accountId}/transactions/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid accountId, Guid id)
        {
            await _mediator.Send(new DeleteTransactionCommand(accountId, id));
            return NoContent();
        }

        // =========================
        // GET ALL (global - dashboard)
        // =========================

        /// <summary>
        /// Gets all transactions across all accounts (for dashboard/reports).
        /// </summary>
        [HttpGet("/api/v1/transactions")]
        [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TransactionDto>>> GetAllTransactions(
            [FromQuery] TransactionType? type,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] Guid? categoryId)
        {
            var result = await _mediator.Send(new GetAllTransactionsQuery(
                type,
                from,
                to,
                categoryId));

            return Ok(result);
        }
    }
}