using FinanceTracker.Application.Interfaces;
using FinanceTracker.Infrastructure.Service.Email;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ResendEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _templatesPath;

    public ResendEmailService(HttpClient httpClient, IOptions<ResendSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
        _templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Service", "Email", "Templates");

    }

    public async Task SendAsync(string to, string subject, string html) 
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var body = new
        {
            from = "FinanceTracker <onboarding@resend.dev>", 
            to = new[] { to },
            subject,
            html
        };

        request.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Email failed: {error}");
        }
    }

    public async Task<string> LoadTemplateAsync(string templateName, Dictionary<string, string> replacements)
    {
        var templatePath = Path.Combine(_templatesPath, templateName);

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Email template not found: {templateName}");

        var html = await File.ReadAllTextAsync(templatePath);

        foreach (var (key, value) in replacements)
        {
            html = html.Replace($"{{{{{key}}}}}", value);
        }

        return html;
    }
}