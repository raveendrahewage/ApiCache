using ApiCache.Application.Interfaces;
using ApiCache.Domain.Entities;
using ApiCache.Helper.Exceptions;
using ApiCache.Infrastructure.External.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace ApiCache.Infrastructure.External.Clients;

public class PostApiClient(HttpClient httpClient, IOptions<PostApiOptions> options) : IPostApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly PostApiOptions _options = options.Value;

    public async Task<Post?> FetchPostByIdAsync(int id)
    {
        var response = await SendRequestAsync(() => _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts/{id}"));
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        return await ReadJsonContentAsync<Post>(response);
    }

    public async Task<IEnumerable<Post>> FetchPostsByUserIdAsync(int userId)
    {
        var response = await SendRequestAsync(() => _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts?userId={userId}"));
        return await ReadJsonContentAsync<IEnumerable<Post>>(response) ?? [];
    }

    public async Task<IEnumerable<Post>> FetchAllPostsAsync()
    {
        var response = await SendRequestAsync(() => _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts"));
        return await ReadJsonContentAsync<IEnumerable<Post>>(response) ?? [];
    }

    private static async Task<HttpResponseMessage> SendRequestAsync(Func<Task<HttpResponseMessage>> requestFunc)
    {
        try
        {
            var response = await requestFunc();

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
            {
                throw new ExternalApiException($"External API error while fetching post(s).", HttpStatusCode.BadGateway);
            }

            return response;
        }
        catch (HttpRequestException)
        {
            throw new ExternalApiException($"External API failure.", HttpStatusCode.BadGateway);
        }
        catch (TaskCanceledException)
        {
            throw new ExternalApiException($"External API request timed out.", HttpStatusCode.GatewayTimeout);
        }
    }

    private static async Task<T?> ReadJsonContentAsync<T>(HttpResponseMessage response)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception)
        {
            throw new ExternalApiException($"Failed to deserialize payload.", HttpStatusCode.BadGateway);
        }
    }
}
