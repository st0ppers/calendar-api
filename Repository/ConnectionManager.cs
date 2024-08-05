using CalendarApi.Internal;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public sealed class ConnectionManager(IOptions<ConnectionString> connectionString)
{
    public Task<Result<T, Exception>> ExecuteAsync<T>(Func<IMongoDatabase, Task<T>> func) =>
        ConnectAsync(func);

    private async Task<Result<T, Exception>> ConnectAsync<T>(Func<IMongoDatabase, Task<T>> func)
    {
        try
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString.Value.Default);
            var client = new MongoClient(settings);
            var db = client.GetDatabase("DungeonsAndDragons");

            return await func(db);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in db: {e.Message}");
            return DatabaseException.New(e);
        }
    }
}

//TODO: Move to different file
public sealed class DatabaseException : Exception
{
    public DatabaseException(string message, Exception e)
        : base(message) { }

    public static DatabaseException New(Exception e) => new DatabaseException($"Error in db", e);
}
