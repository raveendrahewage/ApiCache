# ApiCache

This is a simple ASP.NET Core Web API that fetches posts from the JSONPlaceholder API and caches them in a local SQL Server database. 
It checks the database first. If the post is already saved local database, it returns it. If not, it fetches it from the third-party API, saves it to the database, and returns it.
There is option to toggle between local chached data and third-party api real time data when you fetching multiple posts.
I have added an endpoint to clear the cache if needed.

---

## Prerequisites

* [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
* Install `ASP.NET and web development` workload in Visual Studio via `Visual Studio Installer` if not already installed(This is required to use SQL Server Express LocalDB).

---

## How to Run the Project

1. Open your terminal in a folder that you want project to be and clone the repository:
   ```bash
   git clone https://github.com/raveendrahewage/ApiCache.git
   ```
   ```
   cd ApiCache
   ```
2. Build the project:
   ```bash
   dotnet build
   ```
3. Run the project:
   ```bash
   dotnet run
   ```

Note: Since the application uses `SQL Server Express LocalDB`, you don not need to manually create the database. The app will automatically create the database and the required tables on startup.

## Environment Variables/ Configurations

Since the application uses JSONPlaceholder api and is a free API, you do not need to setup any API keys.

## API Endpoints
```
Get single post by ID: GET /api/posts/{id}
```
```
Get posts by User ID: GET /api/posts/user/{userId}?cachedOnly=(true|false)
```
```
Get all posts: GET /api/posts?cachedOnly=(true|false)
```
```
Delete single cached post by ID: DELETE /api/posts/{id}
```
```
Delete cached posts by User ID: DELETE /api/posts/user/{userId}
```
```
Delete all cached posts: DELETE /api/posts
```

## Libraries Used
* `Swashbuckle.AspNetCore:`To generate Swagger documentation and UI. You will see the swagger UI page when you run the application.

* `Dapper:` To run SQL queries and map them to C# objects.

* `Microsoft.Data.SqlClient:` To connect to SQL Server and manage database commands and connections.

* `Microsoft.Extensions.Http:` To handle external JSONPlaceholder API calls.

* `Microsoft.Extensions.Options:` To bind settings from appsettings.json and environment variables to C# options classes.