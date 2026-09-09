using ApiCache.Helper.Exceptions;
using ApiCache.Helper.Models;
using System.Net;
using System.Text.Json;

namespace ApiCache.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            HumanErrorException => (int)HttpStatusCode.BadRequest,
            ExternalApiException ex => (int)ex.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError
        };
        var message = exception switch
        {
            NotFoundException or HumanErrorException or ExternalApiException => exception.Message,
            _ => "An unexpected error occurred. Please try again later."
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new ApiResponse<object?>(statusCode, message, null));
        return context.Response.WriteAsync(result);
    }
}
