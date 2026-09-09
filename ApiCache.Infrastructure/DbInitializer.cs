using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ApiCache.Infrastructure;

public static class DbInitializer
{
    public static void Initialize(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var builder = new SqlConnectionStringBuilder(connectionString);
        var targetDatabase = builder.InitialCatalog;

        builder.InitialCatalog = "master";

        using var masterConnection = new SqlConnection(builder.ConnectionString);
        const string createDbSql = @"
            IF DB_ID(@DbName) IS NULL
                BEGIN
                    DECLARE @sql NVARCHAR(MAX) = N'CREATE DATABASE [' + @DbName + N']';
                    EXEC sp_executesql @sql;
                END
            ";
        masterConnection.Execute(createDbSql, new { DbName = targetDatabase });

        using var targerConnection = new SqlConnection(connectionString);
        const string createTableSql = @"
            IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'Posts')
                BEGIN
                    CREATE TABLE Posts (
                        Id INT PRIMARY KEY,        
                        UserId INT NOT NULL,
                        Title NVARCHAR(255) NOT NULL,
                        Body NVARCHAR(MAX) NOT NULL,
                        FetchedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
                    );
                END
            ";
        targerConnection.Execute(createTableSql);
    }
}
