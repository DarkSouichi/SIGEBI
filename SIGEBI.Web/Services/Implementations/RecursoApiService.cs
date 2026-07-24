using System.Text.Json;
using SIGEBI.Web.Models.Recurso;

namespace SIGEBI.Web.Services
{
    public class RecursoApiService : IRecursoApiService
    {
        private readonly HttpClient _httpClient;

        public RecursoApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
        }

        public async Task<GetAllRecursosResponse> GetAll()
        {
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