using System.Text.Json;
using SIGEBI.Web.Models.Penalizacion;

namespace SIGEBI.Web.Services
{
    public class PenalizacionApiService : IPenalizacionApiService
    {
        private readonly HttpClient _httpClient;

        public PenalizacionApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
        }

        public async Task<GetAllPenalizacionesResponse> GetAll()
        {
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
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Penalizacion/ActualizarPenalizacion", model);
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