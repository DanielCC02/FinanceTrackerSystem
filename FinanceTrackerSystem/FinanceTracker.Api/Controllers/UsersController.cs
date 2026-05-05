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
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================
        // CREATE USER
        // =========================

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

        // =========================
        // GET USERS (ADMIN)
        // =========================

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

        // =========================
        // GET USER BY ID (ADMIN)
        // =========================

        /// <summary>
        /// Gets a user by ID (Admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return Ok(result);
        }

        // =========================
        // GET MY PROFILE
        // =========================

        /// <summary>
        /// Gets the current user's profile.
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> GetMyProfile()
        {
            var result = await _mediator.Send(new GetMyProfileQuery());
            return Ok(result);
        }

        // =========================
        // UPDATE MY PROFILE
        // =========================

        /// <summary>
        /// Updates the current user's profile.
        /// </summary>
        [HttpPut("me")]
        [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            var updatedUser = await _mediator.Send(command);
            return Ok(updatedUser);
        }

        // =========================
        // UPDATE USER (ADMIN)
        // =========================

        /// <summary>
        /// Updates a user (Admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var updatedUser = await _mediator.Send(command);
            return Ok(updatedUser);
        }

        // =========================
        // UPDATE PASSWORD
        // =========================

        /// <summary>
        /// Updates the current user's password.
        /// </summary>
        [HttpPut("me/password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        // =========================
        // DELETE USER (ADMIN)
        // =========================

        /// <summary>
        /// Deletes a user (Admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }

        // =========================
        // DEACTIVATE MY ACCOUNT 
        // =========================

        /// <summary>
        /// Deactivates the current user's account.
        /// </summary>
        [HttpDelete("me")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeactivateMyAccount([FromBody] DeactivateMyAccountCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}