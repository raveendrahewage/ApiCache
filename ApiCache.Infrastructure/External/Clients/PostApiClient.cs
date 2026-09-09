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
        try
        {
            var response = await SendRequestAsync(() => _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts/{id}"));
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await ReadJsonContentAsync<Post>(response);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Post>> FetchPostsByUserIdAsync(int userId)
    {
        try
        {
            var response = await SendRequestAsync(() => _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts?userId={userId}"));
            response.EnsureSuccessStatusCode();
            return await ReadJsonContentAsync<IEnumerable<Post>>(response) ?? [];
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Post>> FetchAllPostsAsync()
    {
        try
        {
            var response = await SendRequestAsync(() => _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts"));
            response.EnsureSuccessStatusCode();
            return await ReadJsonContentAsync<IEnumerable<Post>>(response) ?? [];
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static async Task<HttpResponseMessage> SendRequestAsync(Func<Task<HttpResponseMessage>> requestFunc)
    {
        try
        {
            var response = await requestFunc();

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
            {
                throw new ExternalApiException($"External API error while.", HttpStatusCode.BadGateway);
            }

            return response;
        }
        catch (HttpRequestException)
        {
            throw new ExternalApiException($"Network failure.", HttpStatusCode.BadGateway);
        }
        catch (TaskCanceledException)
        {
            throw new ExternalApiException($"Request timed out.", HttpStatusCode.GatewayTimeout);
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
