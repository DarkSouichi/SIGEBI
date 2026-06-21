namespace SIGEBI.Domain.Base;

public class OperationResult
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}