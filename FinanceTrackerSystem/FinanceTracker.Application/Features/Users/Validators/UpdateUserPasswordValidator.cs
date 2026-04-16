using FluentValidation;
using FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword;

namespace FinanceTracker.Application.Features.Users.Validators
{
    public class UpdateUserPasswordValidator : AbstractValidator<UpdateUserPasswordCommand>
    {
        public UpdateUserPasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Must contain uppercase letter")
                .Matches("[a-z]").WithMessage("Must contain lowercase letter")
                .Matches("[0-9]").WithMessage("Must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Must contain a special character");
        }
    }
}