using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Notificacion;

namespace SIGEBI.Web.Services
{
    public class NotificacionApiService : INotificacionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioApiService _usuarioApiService;

        public NotificacionApiService(IHttpClientFactory httpClientFactory,
                                      IHttpContextAccessor httpContextAccessor,
                                      IUsuarioApiService usuarioApiService)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
            _httpContextAccessor = httpContextAccessor;
            _usuarioApiService = usuarioApiService;
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
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
            var response = new GetAllNotificacionesResponse();

            try
            {
                var httpResponse = await _httpClient.GetAsync("Notificacion");
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<GetAllNotificacionesResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new GetAllNotificacionesResponse { isSuccess = false, message = "Error al deserializar." };
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    response.isSuccess = false;
                    response.message = errorResponse?.message ?? httpResponse.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound => "No se encontraron registros.",
                        System.Net.HttpStatusCode.Unauthorized => "No tiene permisos.",
                        System.Net.HttpStatusCode.InternalServerError => "Error interno del servidor.",
                        _ => $"Error inesperado: {httpResponse.StatusCode}"
                    };
                }
            }
            catch (TaskCanceledException)
            {
                response.isSuccess = false;
                response.message = "La solicitud tardó demasiado. Verifique su conexión.";
            }
            catch (HttpRequestException)
            {
                response.isSuccess = false;
                response.message = "No se pudo conectar con el servidor. Verifique que la API esté disponible.";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = $"Error inesperado: {ex.Message}";
            }

            await EnriquecerConNombresUsuario(response);

            return response;
        }

        private async Task EnriquecerConNombresUsuario(GetAllNotificacionesResponse response)
        {
            if (response.isSuccess && response.data != null && response.data.Any())
            {
                var usuariosResponse = await _usuarioApiService.GetAll();
                if (usuariosResponse.isSuccess)
                {
                    var usuarioDict = usuariosResponse.data.ToDictionary(u => u.usuarioId, u => u.nombreCompleto);
                    foreach (var item in response.data)
                    {
                        if (usuarioDict.TryGetValue(item.usuarioId, out var nombre))
                            item.nombreUsuario = nombre;
                    }
                }
            }
        }


        public async Task<GetNotificacionResponse> GetById(int id)
        {
            AddAuthorizationHeader();
            var response = new GetNotificacionResponse();

            try
            {
                var httpResponse = await _httpClient.GetAsync($"Notificacion/{id}");
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<GetNotificacionResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new GetNotificacionResponse { isSuccess = false, message = "Error al deserializar." };
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    response.isSuccess = false;
                    response.message = errorResponse?.message ?? httpResponse.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound => "Registro no encontrado.",
                        System.Net.HttpStatusCode.Unauthorized => "No tiene permisos.",
                        System.Net.HttpStatusCode.InternalServerError => "Error interno del servidor.",
                        _ => $"Error inesperado: {httpResponse.StatusCode}"
                    };
                }
            }
            catch (TaskCanceledException)
            {
                response.isSuccess = false;
                response.message = "La solicitud tardó demasiado. Verifique su conexión.";
            }
            catch (HttpRequestException)
            {
                response.isSuccess = false;
                response.message = "No se pudo conectar con el servidor. Verifique que la API esté disponible.";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = $"Error inesperado: {ex.Message}";
            }

            await EnriquecerConNombreUsuario(response);

            return response;
        }

        private async Task EnriquecerConNombreUsuario(GetNotificacionResponse response)
        {
            if (response.isSuccess && response.data != null)
            {
                var usuariosResponse = await _usuarioApiService.GetAll();
                if (usuariosResponse.isSuccess)
                {
                    var usuarioDict = usuariosResponse.data.ToDictionary(u => u.usuarioId, u => u.nombreCompleto);
                    if (usuarioDict.TryGetValue(response.data.usuarioId, out var nombre))
                        response.data.nombreUsuario = nombre;
                }
            }
        }

        public async Task<ApiResponse> Create(NotificacionCreateModel model)
        {
            AddAuthorizationHeader();
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Notificacion/CrearNotificacion", model);
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
                    response.message = errorResponse?.message ?? httpResponse.StatusCode switch
                    {
                        System.Net.HttpStatusCode.BadRequest => "Datos inválidos. Revise los campos.",
                        System.Net.HttpStatusCode.Unauthorized => "No tiene permisos.",
                        System.Net.HttpStatusCode.InternalServerError => "Error interno del servidor.",
                        _ => $"Error inesperado: {httpResponse.StatusCode}"
                    };
                }
            }
            catch (TaskCanceledException)
            {
                response.isSuccess = false;
                response.message = "La solicitud tardó demasiado. Verifique su conexión.";
            }
            catch (HttpRequestException)
            {
                response.isSuccess = false;
                response.message = "No se pudo conectar con el servidor. Verifique que la API esté disponible.";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = $"Error inesperado: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse> Update(NotificacionEditModel model)
        {
            AddAuthorizationHeader();
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Notificacion/ActualizarNotificacion", model);
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
                    response.message = errorResponse?.message ?? httpResponse.StatusCode switch
                    {
                        System.Net.HttpStatusCode.BadRequest => "Datos inválidos. Revise los campos.",
                        System.Net.HttpStatusCode.NotFound => "Registro no encontrado.",
                        System.Net.HttpStatusCode.Unauthorized => "No tiene permisos.",
                        System.Net.HttpStatusCode.InternalServerError => "Error interno del servidor.",
                        _ => $"Error inesperado: {httpResponse.StatusCode}"
                    };
                }
            }
            catch (TaskCanceledException)
            {
                response.isSuccess = false;
                response.message = "La solicitud tardó demasiado. Verifique su conexión.";
            }
            catch (HttpRequestException)
            {
                response.isSuccess = false;
                response.message = "No se pudo conectar con el servidor. Verifique que la API esté disponible.";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = $"Error inesperado: {ex.Message}";
            }

            return response;
        }
        public async Task<ApiResponse> MarcarLeida(int id)
        {
            AddAuthorizationHeader();
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync($"Notificacion/MarcarLeida/{id}", new { });
                var json = await httpResponse.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new ApiResponse { isSuccess = false, message = "Error al deserializar." };
            }
            catch (Exception ex)
            {
                return new ApiResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
        }
    }
}