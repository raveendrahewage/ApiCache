using ApiCache.Domain.Entities;

namespace ApiCache.Application.Interfaces;

public interface IPostApiClient
{
    Task<Post?> FetchPostByIdAsync(int id);
    Task<IEnumerable<Post>> FetchPostsByUserIdAsync(int userId);
    Task<IEnumerable<Post>> FetchAllPostsAsync();
}
