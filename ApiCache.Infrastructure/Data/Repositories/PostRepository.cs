using ApiCache.Application.Interfaces;
using ApiCache.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ApiCache.Infrastructure.Data.Repositories;

public class PostRepository(IConfiguration configuration) : IPostRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    public async Task<Post?> GetPostByIdAsync(int id)
    {
        const string sql = @"
            SELECT *
            FROM Posts
            WHERE Id = @Id";

        using var db = CreateConnection();
        return await db.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
    {
        const string sql = @"
            SELECT *
            FROM Posts
            WHERE UserId = @UserId";

        using var db = CreateConnection();
        return await db.QueryAsync<Post>(sql, new { UserId = userId });
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        const string sql = @"
            SELECT *
            FROM Posts";

        using var db = CreateConnection();
        return await db.QueryAsync<Post>(sql);
    }
}
