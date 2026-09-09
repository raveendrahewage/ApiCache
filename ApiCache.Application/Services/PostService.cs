using ApiCache.Application.Dtos;
using ApiCache.Application.Interfaces;
using ApiCache.Helper.Exceptions;

namespace ApiCache.Application.Services;

public class PostService(IPostRepository postRepository, IPostApiClient postApiClient) : IPostService
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IPostApiClient _postApiClient = postApiClient;
    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        try
        {
            if(id <= 0)
                throw new HumanErrorException("Post ID is invalid.");

            var cachedPost = await _postRepository.GetPostByIdAsync(id);
            if(cachedPost is not null)
                return new PostDto(cachedPost.Id, cachedPost.UserId, cachedPost.Title, cachedPost.Body);
        
            var apiPost = await _postApiClient.FetchPostByIdAsync(id);
            if(apiPost is not null)
            {
                await _postRepository.SavePostAsync(apiPost);
                return new PostDto(apiPost.Id, apiPost.UserId, apiPost.Title, apiPost.Body);
            }

            throw new NotFoundException("Post not found.");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(int userId, bool cachedOnly = true)
    {
        try
        {
            if (userId <= 0)
                throw new HumanErrorException("User ID is invalid.");

            if (cachedOnly)
            {
                var cachedPosts = await _postRepository.GetPostsByUserIdAsync(userId);
                return cachedPosts.Select(p => new PostDto(p.Id, p.UserId, p.Title, p.Body));
            }
            else
            {
                var apiPosts = await _postApiClient.FetchPostsByUserIdAsync(userId);
                await _postRepository.SaveBulkAsync(apiPosts);
                return apiPosts.Select(p => new PostDto(p.Id, p.UserId, p.Title, p.Body));
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<PostDto>> GetAllPostsAsync(bool cachedOnly = true)
    {
        try
        {
            if (cachedOnly)
            {
                var cachedPosts = await _postRepository.GetAllPostsAsync();
                return cachedPosts.Select(p => new PostDto(p.Id, p.UserId, p.Title, p.Body));
            }
            else
            {
                var apiPosts = await _postApiClient.FetchAllPostsAsync();
                await _postRepository.SaveBulkAsync(apiPosts);
                return apiPosts.Select(p => new PostDto(p.Id, p.UserId, p.Title, p.Body));
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeletePostByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new HumanErrorException("Post ID is invalid.");

            return await _postRepository.DeletePostAsync(id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeletePostsByUserIdAsync(int userId)
    {
        try
        {
            if (userId <= 0)
                throw new HumanErrorException("User ID is invalid.");

            return await _postRepository.DeletePostsByUserIdAsync(userId);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> ClearAllPostsAsync()
    {
        try
        {
            return await _postRepository.ClearAllPostAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
