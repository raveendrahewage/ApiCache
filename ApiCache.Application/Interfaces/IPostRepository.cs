using ApiCache.Application.Dtos;
using ApiCache.Domain.Entities;

namespace ApiCache.Application.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<IEnumerable<Post>> GetAllPostsAsync();
}
