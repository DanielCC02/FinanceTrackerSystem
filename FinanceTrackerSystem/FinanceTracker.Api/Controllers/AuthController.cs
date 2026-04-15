using FinanceTracker.Application.Features.Auth.Commands.ForgotPassword;
using FinanceTracker.Application.Features.Auth.Commands.Login;
using FinanceTracker.Application.Features.Auth.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            await _mediator.Send(command);

            // 🔒 importante: no revelar si el email existe
            return Ok(new { message = "If the email exists, a reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Password reset successfully." });
        }
    }
}
