using FinanceTracker.Application.Features.Users.Commands.CreateUser;
using FinanceTracker.Application.Features.Users.Commands.DeleteUser;
using FinanceTracker.Application.Features.Users.Commands.DesactivateMyAccount;
using FinanceTracker.Application.Features.Users.Commands.UpdateProfile;
using FinanceTracker.Application.Features.Users.Commands.UpdateUser;
using FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Application.Features.Users.Queries.GetMyProfile;
using FinanceTracker.Application.Features.Users.Queries.GetUserById;
using FinanceTracker.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    /// <summary>
    /// Manages users and profiles.
    /// </summary>
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Gets all users (Admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Gets the current user's profile.
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetMyProfile()
        {
            var result = await _mediator.Send(new GetMyProfileQuery());
            return Ok(result);
        }

        /// <summary>
        /// Updates the current user's profile.
        /// </summary>
        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            var updatedUser = await _mediator.Send(command);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Updates a user (Admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var updatedUser = await _mediator.Send(command);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Updates the current user's password.
        /// </summary>
        [Authorize]
        [HttpPut("me/password")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a user (Admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Deactivates the current user's account.
        /// </summary>
        [Authorize]
        [HttpDelete("me")]
        public async Task<IActionResult> DeactivateMyAccount([FromBody] DeactivateMyAccountCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}