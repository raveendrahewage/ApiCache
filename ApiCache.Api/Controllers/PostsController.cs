using ApiCache.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCache.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(IPostService postService) : Controller
{
    private readonly IPostService _postService = postService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post is null)
            return NotFound();

        return Ok(post);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPostsByUserId(int userId, [FromQuery] bool cachedOnly = false)
    {
        return Ok(await _postService.GetPostsByUserIdAsync(userId, cachedOnly));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts([FromQuery] bool cachedOnly = false)
    {
        return Ok(await _postService.GetAllPostsAsync(cachedOnly));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostById(int id)
    {
        return Ok(await _postService.DeletePostByIdAsync(id));
    }

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeletePostByUserId(int userId)
    {
        return Ok(await _postService.DeletePostsByUserIdAsync(userId));
    }

    [HttpDelete]
    public async Task<IActionResult> ClearAllPosts()
    {
        return Ok(await _postService.ClearAllPostsAsync());
    }
}
