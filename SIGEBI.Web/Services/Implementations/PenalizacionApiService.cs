using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Penalizacion;
using SIGEBI.Web.Models.Usuario;
using SIGEBI.Web.Models.Prestamo;

namespace SIGEBI.Web.Services
{
    public class PenalizacionApiService : IPenalizacionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly IPrestamoApiService _prestamoApiService;

        public PenalizacionApiService(IHttpClientFactory httpClientFactory,
                                      IHttpContextAccessor httpContextAccessor,
                                      IUsuarioApiService usuarioApiService,
                                      IPrestamoApiService prestamoApiService)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
            _httpContextAccessor = httpContextAccessor;
            _usuarioApiService = usuarioApiService;
            _prestamoApiService = prestamoApiService;
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

        public async Task<GetAllPenalizacionesResponse> GetAll()
        {
            AddAuthorizationHeader();
            var response = new GetAllPenalizacionesResponse();

            try
            {
                var httpResponse = await _httpClient.GetAsync("Penalizacion");
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<GetAllPenalizacionesResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new GetAllPenalizacionesResponse { isSuccess = false, message = "Error al deserializar." };
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

            await EnriquecerConNombres(response);

            return response;
        }

        private async Task EnriquecerConNombres(GetAllPenalizacionesResponse response)
        {
            if (response.isSuccess && response.data != null && response.data.Any())
            {
                var usuariosResponse = await _usuarioApiService.GetAll();
                var usuarioDict = usuariosResponse.isSuccess && usuariosResponse.data != null
                    ? usuariosResponse.data.ToDictionary(u => u.usuarioId, u => u.nombreCompleto)
                    : new Dictionary<int, string>();

                var prestamosResponse = await _prestamoApiService.GetAll();
                var prestamoDict = prestamosResponse.isSuccess && prestamosResponse.data != null
                    ? prestamosResponse.data.ToDictionary(p => p.prestamoId, p => p.codigoEjemplar ?? $"Préstamo #{p.prestamoId}")
                    : new Dictionary<int, string>();

                foreach (var item in response.data)
                {
                    if (usuarioDict.TryGetValue(item.usuarioId, out var nombre))
                        item.nombreUsuario = nombre;

                    if (prestamoDict.TryGetValue(item.prestamoId, out var info))
                        item.prestamoInfo = info;
                    else
                        item.prestamoInfo = $"Préstamo #{item.prestamoId}";
                }
            }
        }

        public async Task<GetPenalizacionResponse> GetById(int id)
        {
            AddAuthorizationHeader();
            var response = new GetPenalizacionResponse();

            try
            {
                var httpResponse = await _httpClient.GetAsync($"Penalizacion/{id}");
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<GetPenalizacionResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new GetPenalizacionResponse { isSuccess = false, message = "Error al deserializar." };
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

            await EnriquecerConNombres(response);

            return response;
        }

        private async Task EnriquecerConNombres(GetPenalizacionResponse response)
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

                var prestamosResponse = await _prestamoApiService.GetAll();
                if (prestamosResponse.isSuccess)
                {
                    var prestamoDict = prestamosResponse.data.ToDictionary(p => p.prestamoId, p => p.codigoEjemplar ?? $"Préstamo #{p.prestamoId}");
                    if (prestamoDict.TryGetValue(response.data.prestamoId, out var info))
                        response.data.prestamoInfo = info;
                    else
                        response.data.prestamoInfo = $"Préstamo #{response.data.prestamoId}";
                }
            }
        }

        public async Task<ApiResponse> Create(PenalizacionCreateModel model)
        {
            AddAuthorizationHeader();
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Penalizacion/CrearPenalizacion", model);
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

        public async Task<ApiResponse> Update(PenalizacionEditModel model)
        {
            AddAuthorizationHeader();
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Penalizacion/ActualizarPenalizacion", model);
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
    }
}