using ApiCache.Application.Interfaces;
using ApiCache.Domain.Entities;
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
            var response = await _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Post>();
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
            var response = await _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts?userId={userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Post>>() ?? [];
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
            var response = await _httpClient.GetAsync($"{_options.BaseUrl.TrimEnd('/')}/posts");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Post>>() ?? [];
        }
        catch (Exception)
        {
            throw;
        }
    }
}
