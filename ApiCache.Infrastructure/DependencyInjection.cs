using ApiCache.Application.Interfaces;
using ApiCache.Application.Services;
using ApiCache.Infrastructure.Data.Repositories;
using ApiCache.Infrastructure.External.Clients;
using ApiCache.Infrastructure.External.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ApiCache.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastruture(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostApiOptions>(configuration.GetSection(PostApiOptions.Position));

        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IPostRepository, PostRepository>();

        services.AddHttpClient<IPostApiClient, PostApiClient>((serviceProvider, client) =>
        {
            var postApiOptions = serviceProvider.GetRequiredService<IOptions<PostApiOptions>>().Value;

            client.BaseAddress = new Uri(postApiOptions.BaseUrl);

            if(!string.IsNullOrEmpty(postApiOptions.ApiKey))
                client.DefaultRequestHeaders.Add("X-Api-Key", postApiOptions.ApiKey);
        });

        return services;
    }
}
