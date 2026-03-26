namespace FinanceTracker.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
