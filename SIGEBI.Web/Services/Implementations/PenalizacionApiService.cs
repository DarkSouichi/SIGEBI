using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Penalizacion;
using SIGEBI.Web.Models.Usuario;
using SIGEBI.Web.Models.Prestamo;
using SIGEBI.Web.Models.Ejemplar;
using SIGEBI.Web.Models.Recurso;

namespace SIGEBI.Web.Services
{
    public class PenalizacionApiService : IPenalizacionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly IEjemplarApiService _ejemplarApiService;  
        private readonly IRecursoApiService _recursoApiService;    

        public PenalizacionApiService(IHttpClientFactory httpClientFactory,
                                      IHttpContextAccessor httpContextAccessor,
                                      IUsuarioApiService usuarioApiService,
                                      IPrestamoApiService prestamoApiService,
                                      IEjemplarApiService ejemplarApiService,  
                                      IRecursoApiService recursoApiService)    
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
            _httpContextAccessor = httpContextAccessor;
            _usuarioApiService = usuarioApiService;
            _prestamoApiService = prestamoApiService;
            _ejemplarApiService = ejemplarApiService;  
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
                    ? prestamosResponse.data.ToDictionary(p => p.prestamoId, p => p)
                    : new Dictionary<int, PrestamoModel>();

                var ejemplaresResponse = await _ejemplarApiService.GetAll();
                var ejemplarCodigoDict = ejemplaresResponse.isSuccess && ejemplaresResponse.data != null
                    ? ejemplaresResponse.data.ToDictionary(e => e.ejemplarId, e => e.codigoBarras)
                    : new Dictionary<int, string>();

                var recursosResponse = await _recursoApiService.GetAll();
                var recursoTituloDict = recursosResponse.isSuccess && recursosResponse.data != null
                    ? recursosResponse.data.ToDictionary(r => r.recursoId, r => r.titulo)
                    : new Dictionary<int, string>();

                var ejemplarTituloDict = new Dictionary<int, string>();
                if (ejemplaresResponse.isSuccess && ejemplaresResponse.data != null &&
                    recursosResponse.isSuccess && recursosResponse.data != null)
                {
                    foreach (var ejemplar in ejemplaresResponse.data)
                    {
                        if (recursoTituloDict.TryGetValue(ejemplar.recursoId, out var titulo))
                        {
                            ejemplarTituloDict[ejemplar.ejemplarId] = titulo;
                        }
                    }
                }

                foreach (var item in response.data)
                {
                    if (usuarioDict.TryGetValue(item.usuarioId, out var nombre))
                        item.nombreUsuario = nombre;

                    if (prestamoDict.TryGetValue(item.prestamoId, out var prestamo))
                    {
                        var codigo = ejemplarCodigoDict.TryGetValue(prestamo.ejemplarId, out var c) ? c : "N/A";
                        var titulo = ejemplarTituloDict.TryGetValue(prestamo.ejemplarId, out var t) ? t : "Sin título";
                        item.prestamoInfo = $"Préstamo #{item.prestamoId} - Código: {codigo} - {titulo}";
                    }
                    else
                    {
                        item.prestamoInfo = $"Préstamo #{item.prestamoId} (no encontrado)";
                    }
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

                    if (response.isSuccess && response.data != null)
                    {
                        await EnriquecerPenalizacionIndividual(response.data);
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

        private async Task EnriquecerPenalizacionIndividual(PenalizacionModel penalizacion)
        {
            var usuariosResponse = await _usuarioApiService.GetAll();
            if (usuariosResponse.isSuccess && usuariosResponse.data != null)
            {
                var usuarioDict = usuariosResponse.data.ToDictionary(u => u.usuarioId, u => u.nombreCompleto);
                if (usuarioDict.TryGetValue(penalizacion.usuarioId, out var nombre))
                    penalizacion.nombreUsuario = nombre;
            }

            var prestamosResponse = await _prestamoApiService.GetAll();
            if (prestamosResponse.isSuccess && prestamosResponse.data != null)
            {
                var prestamo = prestamosResponse.data.FirstOrDefault(p => p.prestamoId == penalizacion.prestamoId);
                if (prestamo != null)
                {
                    var ejemplaresResponse = await _ejemplarApiService.GetAll();
                    string codigo = "N/A";
                    if (ejemplaresResponse.isSuccess && ejemplaresResponse.data != null)
                    {
                        var ejemplar = ejemplaresResponse.data.FirstOrDefault(e => e.ejemplarId == prestamo.ejemplarId);
                        codigo = ejemplar?.codigoBarras ?? "N/A";
                    }

                    string titulo = "Sin título";
                    if (ejemplaresResponse.isSuccess && ejemplaresResponse.data != null)
                    {
                        var ejemplar = ejemplaresResponse.data.FirstOrDefault(e => e.ejemplarId == prestamo.ejemplarId);
                        if (ejemplar != null)
                        {
                            var recursosResponse = await _recursoApiService.GetAll();
                            if (recursosResponse.isSuccess && recursosResponse.data != null)
                            {
                                var recurso = recursosResponse.data.FirstOrDefault(r => r.recursoId == ejemplar.recursoId);
                                titulo = recurso?.titulo ?? "Sin título";
                            }
                        }
                    }

                    penalizacion.prestamoInfo = $"Préstamo #{penalizacion.prestamoId} - Código: {codigo} - {titulo}";
                }
                else
                {
                    penalizacion.prestamoInfo = $"Préstamo #{penalizacion.prestamoId} (no encontrado)";
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