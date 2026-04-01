using FinanceTracker.Application.Features.Users.Commands.CreateUser;
using FinanceTracker.Application.Features.Users.Commands.DeleteUser;
using FinanceTracker.Application.Features.Users.Commands.UpdateUser;
using FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Application.Features.Users.Queries.GetUserById;
using FinanceTracker.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetUserByIdQuery(id));
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            try
            {
                var updatedUser = await _mediator.Send(command);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, [FromBody] UpdateUserPasswordCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
