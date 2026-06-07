namespace SIGEBI.Domain.Base;

public class OperationResult
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public object? Datos { get; set; }

    public static OperationResult Ok(string mensaje = "OK", object? datos = null)
        => new() { Exitoso = true, Mensaje = mensaje, Datos = datos };

    public static OperationResult Fail(string mensaje)
        => new() { Exitoso = false, Mensaje = mensaje };
}