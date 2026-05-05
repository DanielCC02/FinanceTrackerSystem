
namespace FinanceTracker.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string html);
        Task<string> LoadTemplateAsync(string templateName, Dictionary<string, string> replacements);
    }
}
