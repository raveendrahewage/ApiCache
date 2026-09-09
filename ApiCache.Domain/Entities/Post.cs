using ApiCache.Helper.Models;

namespace ApiCache.Domain.Entities;

public class Post: DataRecord
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
