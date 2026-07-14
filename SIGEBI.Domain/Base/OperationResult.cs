namespace SIGEBI.Domain.Base;

public class OperationResult
{
    public bool IsSuccess { get; set; } = true;
    public bool Success
    {
        get => IsSuccess;
        set => IsSuccess = value;
    }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}