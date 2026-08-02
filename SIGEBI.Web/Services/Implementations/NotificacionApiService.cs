using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models.Notificacion;

namespace SIGEBI.Web.Services
{
    public class NotificacionApiService : INotificacionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificacionApiService(IHttpClientFactory httpClientFactory,
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

        public async Task<GetAllNotificacionesResponse> GetAll()
        {
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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

        public async Task<ApiResponse> Update(NotificacionEditModel model)
        {
            AddAuthorizationHeader();
            ApiResponse response = null;
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Notificacion/ActualizarNotificacion", model);
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