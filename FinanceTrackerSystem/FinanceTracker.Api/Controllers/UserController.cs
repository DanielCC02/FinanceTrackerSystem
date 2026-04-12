using FinanceTracker.Application.Features.Users.Commands.CreateUser;
using FinanceTracker.Application.Features.Users.Commands.DeleteUser;
using FinanceTracker.Application.Features.Users.Commands.DesactivateMyAccount;
using FinanceTracker.Application.Features.Users.Commands.UpdateUser;
using FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Application.Features.Users.Queries.GetUserById;
using FinanceTracker.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var updatedUser = await _mediator.Send(command);
            return Ok(updatedUser);
        }

        [Authorize]
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, UpdateUserPasswordCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }

        [HttpPost("deactivate")]
        [Authorize]
        public async Task<IActionResult> DeactivateMyAccount(DeactivateMyAccountCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}