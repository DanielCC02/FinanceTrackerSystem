using FluentValidation;
using FinanceTracker.Application.Features.Auth.Commands.ResetPassword;

namespace FinanceTracker.Application.Features.Auth.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Must contain uppercase letter")
                .Matches("[a-z]").WithMessage("Must contain lowercase letter")
                .Matches("[0-9]").WithMessage("Must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Must contain a special character");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match");
        }
    }
}