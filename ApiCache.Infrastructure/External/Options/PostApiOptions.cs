namespace ApiCache.Infrastructure.External.Options;

public class PostApiOptions
{
    public const string Position = "ExternalApis:PostApi";
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
