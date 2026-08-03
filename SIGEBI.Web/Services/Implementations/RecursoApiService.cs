using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models.Recurso;

namespace SIGEBI.Web.Services
{
    public class RecursoApiService : IRecursoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecursoApiService(IHttpClientFactory httpClientFactory,
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

        public async Task<GetAllRecursosResponse> GetAll()
        {
            AddAuthorizationHeader();
            GetAllRecursosResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync("Recurso");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetAllRecursosResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetAllRecursosResponse { isSuccess = false, message = "Error obteniendo recursos." };
                }
            }
            catch (Exception ex)
            {
                response = new GetAllRecursosResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<GetRecursoResponse> GetById(int id)
        {
            AddAuthorizationHeader();
            GetRecursoResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync($"Recurso/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetRecursoResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetRecursoResponse { isSuccess = false, message = "Error obteniendo el recurso." };
                }
            }
            catch (Exception ex)
            {
                response = new GetRecursoResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<ApiResponse> Create(RecursoCreateModel model)
        {
            AddAuthorizationHeader();
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Recurso/CrearRecurso", model);
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

        public async Task<ApiResponse> Update(RecursoEditModel model)
        {
            AddAuthorizationHeader();
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Recurso/ActualizarRecurso", model);
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