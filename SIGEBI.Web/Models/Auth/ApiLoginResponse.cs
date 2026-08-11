namespace SIGEBI.Web.Models.Auth
{
    public class ApiLoginResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public LoginData data { get; set; }
    }

    public class LoginData
    {
        public string token { get; set; }
        public string nombreCompleto { get; set; }
        public string email { get; set; }
        public string rol { get; set; }
        public int usuarioId { get; set; }
        public DateTime expiracion { get; set; }
    }
}