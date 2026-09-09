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

        try
        {
            using var db = CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id });
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
    {
        const string sql = @"
            SELECT *
            FROM Posts
            WHERE UserId = @UserId";

        try
        {
            using var db = CreateConnection();
            return await db.QueryAsync<Post>(sql, new { UserId = userId });
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        const string sql = @"
            SELECT *
            FROM Posts";

        try
        {
            using var db = CreateConnection();
            return await db.QueryAsync<Post>(sql);
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<bool> SavePostAsync(Post post)
    {
        const string sql = @"
            IF NOT EXISTS (SELECT 1 FROM Posts WHERE Id = @Id)
            BEGIN
                INSERT INTO Posts (
                    Id,
                    UserId,
                    Title,
                    Body,
                    FetchedAt,
                )
                VALUES (
                    @Id,
                    @UserId,
                    @Title,
                    @Body,
                    @FetchedAt
                );
            END";

        try
        {
            using var db = CreateConnection();
            var affectedRows = await db.ExecuteAsync(sql, post);
            return affectedRows > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> SaveBulkAsync(IEnumerable<Post> posts)
    {
        if (!posts.Any())
            return true;

        const string sql = @"
            IF NOT EXISTS (SELECT 1 FROM Posts WHERE Id = @Id)
            BEGIN
                INSERT INTO Posts (
                    Id,
                    UserId,
                    Title,
                    Body,
                    FetchedAt,
                )
                VALUES (
                    @Id,
                    @UserId,
                    @Title,
                    @Body,
                    @FetchedAt
                );
            END";

        using var db = CreateConnection();
        db.Open();
        using var transaction = db.BeginTransaction();

        try
        {
            await db.ExecuteAsync(sql, posts, transaction);
            transaction.Commit();
            return true;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        const string sql = @"
            DELETE FROM Posts
            WHERE Id = @Id";

        try
        {
            using var db = CreateConnection();
            var effectedRows = await db.ExecuteAsync(sql, new { Id = id });
            return effectedRows > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeletePostsByUserIdAsync(int userId)
    {
        const string sql = @"
            DELETE FROM Posts
            WHERE UserId = @UserId";

        try
        {
            using var db = CreateConnection();
            var effectedRows = await db.ExecuteAsync(sql, new { UserId = userId });
            return effectedRows > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> ClearAllPostAsync()
    {
        const string sql = @"
            DELETE FROM Posts";

        try
        {
            using var db = CreateConnection();
            var affectedRows = await db.ExecuteAsync(sql);
            return affectedRows > 0;
        }
        catch(Exception)
        {
            throw;
        }
    }
}
