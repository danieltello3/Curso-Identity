using Blazored.Toast.Services;
using Galaxy.Security.UI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Galaxy.Security.UI.Pages
{
    public partial class ConsultarReclamo
    {
        [Inject] HttpClient HttpClient { get; set; } = default!;
        [Inject] IToastService Toast { get; set; } = default!;
        [Inject] NavigationManager Navegador { get; set; } = default!;
        public ConsultarReclamoRequest ReclamoModel { get; set; } = new(); 

        public async Task OnConsultarReclamo()
        {
            try
            {
                
                var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Reclamo/Get/{ReclamoModel.Codigo}");

                request.SetBrowserRequestOption("credentials", "include");

                var response = await HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<GetReclamoResponse>>(options);
                    if (result != null && result.Result != null)
                    {
                        ReclamoModel.Resultado = result.Result;
                        Toast.ShowSuccess("Reclamo consultado exitosamente");
                    }
                    else
                    {
                        ClearForm();
                        Toast.ShowError($"No se pudo obtener la información del reclamo {ReclamoModel.Codigo}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearForm();
                    Toast.ShowWarning("Usted no se encuentra autorizado");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Toast.ShowError($"Hubo un error al consultar el reclamo {ReclamoModel.Codigo}");
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Hubo un error al consultar el reclamo {ReclamoModel.Codigo}");
                ClearForm();
            }
        }

        public void GoToMenu() => Navegador.NavigateTo("/");

        public void ClearForm()
        {
            ReclamoModel = new ConsultarReclamoRequest();
        }

    }
}
