using CalendarApi.Internal;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CalendarApi.Repository;

public sealed class ConnectionManager(IOptions<ConnectionString> connectionString)
{
    public Task<Result<T, Exception>> ExecuteAsync<T>(Func<IMongoDatabase, Task<T>> func) => ConnectAsync(func);

    public Task<Result<T, Exception>> ExecuteCollectionAsync<T, R>(Func<IMongoCollection<R>, Task<T>> func, string collectionName = Constants.Database.Login) =>
        ConnectCollectionAsync(func, collectionName);

    private async Task<Result<T, Exception>> ConnectAsync<T>(Func<IMongoDatabase, Task<T>> func)
    {
        try
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString.Value.Default);
            var client = new MongoClient(settings);
            var db = client.GetDatabase(Constants.Database.Name);

            return await func(db);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in db: {e.Message}");
            return DatabaseException.New(e);
        }
    }

    private async Task<Result<T, Exception>> ConnectCollectionAsync<T, TR>(Func<IMongoCollection<TR>, Task<T>> func, string collectionName)
    {
        try
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString.Value.Default);
            var client = new MongoClient(settings);
            var db = client.GetDatabase(Constants.Database.Name);
            var collection = db.GetCollection<TR>(collectionName);

            return await func(collection);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in db: {e.Message}");
            return DatabaseException.New(e);
        }
    }
}