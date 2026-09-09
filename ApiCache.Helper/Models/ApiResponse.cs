namespace ApiCache.Helper.Models;


public class ApiResponse<T>
{
    public ApiResponse() { }

    public ApiResponse(int statusCode, string message, T? data)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}
