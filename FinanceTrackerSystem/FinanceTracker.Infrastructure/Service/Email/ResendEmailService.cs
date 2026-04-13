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

    public ResendEmailService(HttpClient httpClient, IOptions<ResendSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
    }

    public async Task SendAsync(string to, string subject, string html) 
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var body = new
        {
            from = "FinanceTracker <onboarding@resend.dev>", // luego cambias dominio
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
}