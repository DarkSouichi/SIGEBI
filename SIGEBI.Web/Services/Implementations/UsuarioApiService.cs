using System.Text.Json;
using SIGEBI.Web.Models.Usuario;

namespace SIGEBI.Web.Services
{
    public class UsuarioApiService : IUsuarioApiService
    {
        private readonly HttpClient _httpClient;

        public UsuarioApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
        }

        public async Task<GetAllUsuariosResponse> GetAll()
        {
            GetAllUsuariosResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync("Usuario");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetAllUsuariosResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetAllUsuariosResponse { isSuccess = false, message = "Error obteniendo usuarios." };
                }
            }
            catch (Exception ex)
            {
                response = new GetAllUsuariosResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<GetUsuarioResponse> GetById(int id)
        {
            GetUsuarioResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync($"Usuario/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetUsuarioResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetUsuarioResponse { isSuccess = false, message = "Error obteniendo el usuario." };
                }
            }
            catch (Exception ex)
            {
                response = new GetUsuarioResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<ApiResponse> Create(UsuarioCreateModel model)
        {
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Usuario/CrearUsuario", model);
                var json = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<ApiResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                response = new ApiResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<ApiResponse> Update(UsuarioEditModel model)
        {
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Usuario/ActualizarUsuario", model);
                var json = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<ApiResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                response = new ApiResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }
    }
}