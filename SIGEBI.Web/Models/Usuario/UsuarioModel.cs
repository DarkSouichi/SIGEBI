namespace SIGEBI.Web.Models.Usuario
{
    public class UsuarioModel
    {
        public int usuarioId { get; set; }
        public string nombreCompleto { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public bool estaActivo { get; set; }
        public int rolId { get; set; }
    }

    public class GetAllUsuariosResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public List<UsuarioModel> data { get; set; }
    }

    public class GetUsuarioResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public UsuarioModel data { get; set; }
    }

    public class UsuarioEditModel
    {
        public int usuarioId { get; set; }
        public string nombreCompleto { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public bool estaActivo { get; set; }
        public int rolId { get; set; }
        public DateTime changeDate { get; set; }
        public int changeUser { get; set; }
    }

    public class UsuarioCreateModel
    {
        public string nombreCompleto { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public bool estaActivo { get; set; }
        public int rolId { get; set; }
        public DateTime changeDate { get; set; }
        public int changeUser { get; set; }
    }

    public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}