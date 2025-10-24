using Galaxy.Security.Domain.Dpo;
using Galaxy.Security.Domain.OutPort.Services;
using System.Net.Http.Json;

namespace Galaxy.Security.Infraestructure.Adapters.Services
{
    public class EmailApiService : IEmailApiService
    {
        private readonly HttpClient _httpClient;

        public EmailApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(SendEmailDpo request)
        {
            var response = await _httpClient.PostAsJsonAsync("Notifications/SendEmail", request);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error sending email: {content} - {response.StatusCode}");
            }
        }
    }
}
