using System.Text.Json;
using SIGEBI.Web.Models.Notificacion;

namespace SIGEBI.Web.Services
{
    public class NotificacionApiService : INotificacionApiService
    {
        private readonly HttpClient _httpClient;

        public NotificacionApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
        }

        public async Task<GetAllNotificacionesResponse> GetAll()
        {
            GetAllNotificacionesResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync("Notificacion");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetAllNotificacionesResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetAllNotificacionesResponse { isSuccess = false, message = "Error obteniendo notificaciones." };
                }
            }
            catch (Exception ex)
            {
                response = new GetAllNotificacionesResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<GetNotificacionResponse> GetById(int id)
        {
            GetNotificacionResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync($"Notificacion/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetNotificacionResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetNotificacionResponse { isSuccess = false, message = "Error obteniendo la notificación." };
                }
            }
            catch (Exception ex)
            {
                response = new GetNotificacionResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<ApiResponse> Create(NotificacionCreateModel model)
        {
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Notificacion/CrearNotificacion", model);
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