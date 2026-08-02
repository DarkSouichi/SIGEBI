using System.Text.Json;
using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Ejemplar;


namespace SIGEBI.Web.Services
{
    public class EjemplarApiService : IEjemplarApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EjemplarApiService(IHttpClientFactory httpClientFactory,
                                  IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBIApi");
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            Console.WriteLine($"Token: {token ?? "null"}");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<GetAllEjemplaresResponse> GetAll()
        {
            AddAuthorizationHeader();
            GetAllEjemplaresResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync("Ejemplar");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetAllEjemplaresResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetAllEjemplaresResponse { isSuccess = false, message = "Error obteniendo ejemplares." };
                }
            }
            catch (Exception ex)
            {
                response = new GetAllEjemplaresResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<GetEjemplarResponse> GetById(int id)
        {
            AddAuthorizationHeader();
            GetEjemplarResponse response = null;
            try
            {
                var httpResponse = await _httpClient.GetAsync($"Ejemplar/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<GetEjemplarResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    response = new GetEjemplarResponse { isSuccess = false, message = "Error obteniendo el ejemplar." };
                }
            }
            catch (Exception ex)
            {
                response = new GetEjemplarResponse { isSuccess = false, message = $"Error: {ex.Message}" };
            }
            return response;
        }

        public async Task<ApiResponse> Create(EjemplarCreateModel model)
        {
            AddAuthorizationHeader();

            ApiResponse response = null;

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Ejemplar/CrearEjemplar", model);

                var json = await httpResponse.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<ApiResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (Exception ex)
            {
                response = new ApiResponse
                {
                    isSuccess = false,
                    message = $"Error: {ex.Message}"
                };
            }

            return response;
        }

        public async Task<ApiResponse> Update(EjemplarEditModel model)
        {
            AddAuthorizationHeader();

            ApiResponse response = null;

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("Ejemplar/ActualizarEjemplar", model);

                var json = await httpResponse.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<ApiResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (Exception ex)
            {
                response = new ApiResponse
                {
                    isSuccess = false,
                    message = $"Error: {ex.Message}"
                };
            }

            return response;
        }
    }
}