using ApiCache.Application.Dtos;
using ApiCache.Domain.Entities;

namespace ApiCache.Application.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<bool> SavePostAsync(Post post);
    Task<bool> SaveBulkAsync(IEnumerable<Post> posts);
    Task<bool> DeletePostAsync(int id);
    Task<bool> DeletePostsByUserIdAsync(int userId);
    Task<bool> ClearAllPostAsync();
}
