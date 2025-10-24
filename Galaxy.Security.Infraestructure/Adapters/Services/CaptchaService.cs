using Galaxy.Security.Domain.Dpo;
using Galaxy.Security.Domain.OutPort.Secrets;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Galaxy.Security.Infraestructure.Adapters.Services
{
    public class CaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly IVaultSecretsProvider _vaultService;
        public CaptchaService(HttpClient httpClient, IVaultSecretsProvider vaultSecretsProvider)
        {
            _httpClient = httpClient;
            _vaultService = vaultSecretsProvider;
        }

        public async Task<(string json, bool success)> RedeemAsync(Redeem request)
        {
            var secrets = _vaultService.GetSecretsAsync().GetAwaiter().GetResult();
            var accessToken = secrets["AccessTokenCap"];
            var sidekey = secrets["SideKey"];
            var url = $"http://localhost:3000/{sidekey}/redeem";


            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"Bot {accessToken}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return ("", false);
            }

            var json = JsonDocument.Parse(result).RootElement;
            return (json.ToString(), true);
        }
    }
}
