using Blazored.Toast.Services;
using Galaxy.Security.UI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Galaxy.Security.UI.Pages
{
    public partial class RegistrarReclamo
    {
        [Inject] HttpClient HttpClient { get; set; } = default!;
        [Inject] IToastService Toast { get; set; } = default!;
        [Inject] NavigationManager Navegador { get; set; } = default!;
        public CreateReclamoRequest ReclamoModel { get; set; } = new(); 

        public async Task OnRegistrarReclamo()
        {
            try
            {
                
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/Reclamo/Create")
                {
                    Content = JsonContent.Create(ReclamoModel)
                };

                request.SetBrowserRequestOption("credentials", "include");

                var response = await HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<IdentityResponse>>(options);
                    Toast.ShowSuccess($"El reclamo {result?.Result?.Data} se creo correctamente");
                    Navegador.NavigateTo("/");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Toast.ShowWarning("Usted no se encuentra autorizado");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Toast.ShowError($"Hubo un error al crear el reclamo: {error}");
                }
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Hubo un error al crear el reclamo: {ex.Message}");
            }
        }

        public void GoToMenu() => Navegador.NavigateTo("/");

    }
}
