using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models;
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

        public async Task<GetAllRecursosResponse> GetAll()
        {
            AddAuthorizationHeader();
            var response = new GetAllRecursosResponse();

            try
            {
                var httpResponse = await _httpClient.GetAsync("Recurso");
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<GetAllRecursosResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new GetAllRecursosResponse { isSuccess = false, message = "Error al deserializar." };
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

            return response;
        }

        public async Task<GetRecursoResponse> GetById(int id)
        {
            AddAuthorizationHeader();
            var response = new GetRecursoResponse();

            try
            {
                var httpResponse = await _httpClient.GetAsync($"Recurso/{id}");
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<GetRecursoResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new GetRecursoResponse { isSuccess = false, message = "Error al deserializar." };
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

            return response;
        }

        public async Task<ApiResponse> Create(RecursoCreateModel model)
        {
            AddAuthorizationHeader();
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Recurso/CrearRecurso", model);
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

        public async Task<ApiResponse> Update(RecursoEditModel model)
        {
            AddAuthorizationHeader();
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Recurso/ActualizarRecurso", model);
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