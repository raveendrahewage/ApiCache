using ApiCache.Application.Dtos;

namespace ApiCache.Application.Interfaces;

public interface IPostService
{
    Task<PostDto?> GetPostByIdAsync(int id);
    Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(int userId, bool cachedOnly = false);
    Task<IEnumerable<PostDto>> GetAllPostsAsync(bool cachedOnly = false);
    Task<bool> DeletePostByIdAsync(int id);
    Task<bool> DeletePostsByUserIdAsync(int userId);
    Task<bool> ClearAllPostsAsync();
}
