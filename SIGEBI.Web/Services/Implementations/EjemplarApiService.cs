using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Ejemplar;
using SIGEBI.Web.Models.Recurso;

namespace SIGEBI.Web.Services
{
    public class EjemplarApiService : IEjemplarApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRecursoApiService _recursoApiService;

        public EjemplarApiService(IHttpClientFactory httpClientFactory,
                                  IHttpContextAccessor httpContextAccessor,
                                  IRecursoApiService recursoApiService)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
            _httpContextAccessor = httpContextAccessor;
            _recursoApiService = recursoApiService;
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

        public async Task<GetAllEjemplaresResponse> GetAll()
        {
            AddAuthorizationHeader();
            var response = new GetAllEjemplaresResponse();

            try
            {
                var httpResponse = await _httpClient.GetAsync("Ejemplar");
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<GetAllEjemplaresResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new GetAllEjemplaresResponse { isSuccess = false, message = "Error al deserializar." };
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

            await EnriquecerConTituloRecurso(response);

            return response;
        }

        private async Task EnriquecerConTituloRecurso(GetAllEjemplaresResponse response)
        {
            if (response.isSuccess && response.data != null && response.data.Any())
            {
                var recursosResponse = await _recursoApiService.GetAll();
                if (recursosResponse.isSuccess && recursosResponse.data != null)
                {
                    var recursoDict = recursosResponse.data.ToDictionary(r => r.recursoId, r => r.titulo);
                    foreach (var item in response.data)
                    {
                        if (recursoDict.TryGetValue(item.recursoId, out var titulo))
                            item.tituloRecurso = titulo;
                    }
                }
            }
        }

        public async Task<GetEjemplarResponse> GetById(int id)
        {
            AddAuthorizationHeader();
            var response = new GetEjemplarResponse();

            try
            {
                var httpResponse = await _httpClient.GetAsync($"Ejemplar/{id}");
                var json = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonSerializer.Deserialize<GetEjemplarResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new GetEjemplarResponse { isSuccess = false, message = "Error al deserializar." };

                    if (response.isSuccess && response.data != null)
                    {
                        var recursosResponse = await _recursoApiService.GetAll();
                        if (recursosResponse.isSuccess && recursosResponse.data != null)
                        {
                            var recursoDict = recursosResponse.data.ToDictionary(r => r.recursoId, r => r.titulo);
                            if (recursoDict.TryGetValue(response.data.recursoId, out var titulo))
                                response.data.tituloRecurso = titulo;
                        }
                    }
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

        public async Task<ApiResponse> Create(EjemplarCreateModel model)
        {
            AddAuthorizationHeader();
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Ejemplar/CrearEjemplar", model);
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

        public async Task<ApiResponse> Update(EjemplarEditModel model)
        {
            AddAuthorizationHeader();
            var response = new ApiResponse();

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Ejemplar/ActualizarEjemplar", model);
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