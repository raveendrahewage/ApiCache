namespace ApiCache.Application.Dtos;

public class PostDto
{
    public PostDto() { }
    public PostDto(int id, int userId, string title, string body)
    {
        Id = id;
        UserId = userId;
        Title = title;
        Body = body;
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
