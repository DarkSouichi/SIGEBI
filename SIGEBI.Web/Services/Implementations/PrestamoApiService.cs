using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models.Prestamo;

namespace SIGEBI.Web.Services
{
    public class PrestamoApiService : IPrestamoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PrestamoApiService(IHttpClientFactory httpClientFactory,
                                  IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<GetAllPrestamosResponse> GetAll()
        {
            AddAuthorizationHeader();
            GetAllPrestamosResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync("Prestamo");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetAllPrestamosResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetAllPrestamosResponse { isSuccess = false, message = "Error obteniendo préstamos." };
                }
            }
            catch (Exception ex)
            {
                response = new GetAllPrestamosResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<GetPrestamoResponse> GetById(int id)
        {
            AddAuthorizationHeader();
            GetPrestamoResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync($"Prestamo/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetPrestamoResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetPrestamoResponse { isSuccess = false, message = "Error obteniendo el préstamo." };
                }
            }
            catch (Exception ex)
            {
                response = new GetPrestamoResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<ApiResponse> Create(PrestamoCreateModel model)
        {
            AddAuthorizationHeader();
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Prestamo/CrearPrestamo", model);
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

        public async Task<ApiResponse> Update(PrestamoEditModel model)
        {
            AddAuthorizationHeader();
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Prestamo/ActualizarPrestamo", model);
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