namespace ApiCache.Infrastructure.External.Options;

public class PostApiOptions
{
    public string Position { get; set; } = "ExternalApis";
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
