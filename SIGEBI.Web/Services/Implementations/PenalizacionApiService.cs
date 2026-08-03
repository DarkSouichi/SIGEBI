using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models.Penalizacion;

namespace SIGEBI.Web.Services
{
    public class PenalizacionApiService : IPenalizacionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PenalizacionApiService(IHttpClientFactory httpClientFactory,
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

        public async Task<GetAllPenalizacionesResponse> GetAll()
        {
            AddAuthorizationHeader();
            GetAllPenalizacionesResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync("Penalizacion");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetAllPenalizacionesResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetAllPenalizacionesResponse { isSuccess = false, message = "Error obteniendo penalizaciones." };
                }
            }
            catch (Exception ex)
            {
                response = new GetAllPenalizacionesResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<GetPenalizacionResponse> GetById(int id)
        {
            AddAuthorizationHeader();
            GetPenalizacionResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync($"Penalizacion/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetPenalizacionResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetPenalizacionResponse { isSuccess = false, message = "Error obteniendo la penalización." };
                }
            }
            catch (Exception ex)
            {
                response = new GetPenalizacionResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<ApiResponse> Create(PenalizacionCreateModel model)
        {
            AddAuthorizationHeader();
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Penalizacion/CrearPenalizacion", model);
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

        public async Task<ApiResponse> Update(PenalizacionEditModel model)
        {
            AddAuthorizationHeader();
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Penalizacion/ActualizarPenalizacion", model);
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<ApiResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = JsonSerializer.Deserialize<ApiResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (response == null)
                    {
                        response = new ApiResponse { isSuccess = false, message = $"Error {httpResponse.StatusCode}: {json}" };
                    }
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }
    }
}