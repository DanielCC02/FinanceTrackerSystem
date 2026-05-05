using FinanceTracker.Application.Features.Auth.Commands.ConfirmEmail;
using FinanceTracker.Application.Features.Auth.Commands.ForgotPassword;
using FinanceTracker.Application.Features.Auth.Commands.Login;
using FinanceTracker.Application.Features.Auth.Commands.ResendConfirmation;
using FinanceTracker.Application.Features.Auth.Commands.ResetPassword;
using FinanceTracker.Application.Features.Auth.Commands.SendEmailConfirmation;
using FinanceTracker.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Controllers
{
    /// <summary>
    /// Handles authentication and password management.
    /// </summary>
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // =========================
        // LOGIN
        // =========================

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // =========================
        // CONFIRM EMAIL
        // =========================

        /// <summary>
        /// Confirms the user's email using a valid token.
        /// </summary>
        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Email confirmed successfully." });
        }

        /// <summary>
        /// Resends the email confirmation link.
        /// </summary>
        [HttpPost("resend-confirmation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "If the email exists and is not confirmed, a new link has been sent." });
        }

        // =========================
        // FORGOT PASSWORD
        // =========================

        /// <summary>
        /// Sends a password reset email if the account exists.
        /// Always returns 200 to avoid user enumeration.
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "If the email exists, a reset link has been sent." });
        }

        // =========================
        // RESET PASSWORD
        // =========================

        /// <summary>
        /// Resets the user's password using a valid reset token.
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Password reset successfully." });
        }
    }
}