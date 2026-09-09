using System.Net;

namespace ApiCache.Helper.Exceptions;

public class ExternalApiException(string message, HttpStatusCode statusCode = HttpStatusCode.BadGateway):Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}
