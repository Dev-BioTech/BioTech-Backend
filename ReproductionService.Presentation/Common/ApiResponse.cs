
using System.Text.Json.Serialization;

namespace ReproductionService.Presentation.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public ApiResponse() {}

    public ApiResponse(T data, string message = null)
    {
        Success = true;
        Message = message;
        Data = data;
    }

    public ApiResponse(string message, List<string> errors = null)
    {
        Success = false;
        Message = message;
        Errors = errors;
    }

    public static ApiResponse<T> Ok(T data, string message = null)
    {
        return new ApiResponse<T>(data, message);
    }

    public static ApiResponse<T> Fail(string message, List<string> errors = null)
    {
        return new ApiResponse<T>(message, errors);
    }
}
