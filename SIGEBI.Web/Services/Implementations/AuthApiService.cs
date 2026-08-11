using System.Text.Json;
using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Auth;

namespace SIGEBI.Web.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
        }

        public async Task<ApiResponse> Register(RegisterViewModel model)
        {
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Auth/Register", model);
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<ApiResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new ApiResponse { isSuccess = false, message = "Error al deserializar." };
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    response.isSuccess = false;
                    response.message = errorResponse?.message ?? $"Error {httpResponse.StatusCode}";
                }
            }
            catch (HttpRequestException)
            {
                response.isSuccess = false;
                response.message = "No se pudo conectar con el servidor.";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = $"Error inesperado: {ex.Message}";
            }

            return response;
        }

        public async Task<LoginResponseViewModel> Login(string email, string password)
        {
            var loginDto = new { Email = email, Password = password };
            var response = new LoginResponseViewModel();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Auth/Login", loginDto);
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (!IsValidJson(json))
                {
                    response.isSuccess = false;
                    response.message = json;
                    return response;
                }

                var result = JsonSerializer.Deserialize<ApiLoginResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.isSuccess == true && result.data != null)
                {
                    response.isSuccess = true;
                    response.token = result.data.token;
                    response.nombreCompleto = result.data.nombreCompleto;
                    response.rol = result.data.rol;
                    response.usuarioId = result.data.usuarioId;
                    response.message = "Login exitoso.";
                }
                else
                {
                    response.isSuccess = false;
                    response.message = result?.message ?? "Error al iniciar sesión.";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = $"Error: {ex.Message}";
            }

            return response;
        }

        private bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) return false;
            strInput = strInput.Trim();
            return (strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                   (strInput.StartsWith("[") && strInput.EndsWith("]"));
        }
    }
}