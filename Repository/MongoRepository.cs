using System.Text.Json;
using CalendarApi.Contracts.Entities;
using CalendarApi.Internal;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public interface IMongoRepository
{
    public Task<IEnumerable<Entity>> GetAll();
    public Task<Result<PlayerEntity, Exception>> GetPlayer(LoginRequest request);
    public Task<Result<bool, Exception>> RegisterPlayer(LoginRequest request);
}

public sealed class MongoRepository : IMongoRepository
{
    //TODO: Maybe move to constants
    private const string Database = "DungeonsAndDragons";
    private const string Calendar = "Calendar";
    private const string Login = "Login";

    private readonly ConnectionString _connectionString;
    private readonly ConnectionManager _connectionManager;

    public MongoRepository(
        IOptions<ConnectionString> connectionString,
        ConnectionManager connectionManager
    )
    {
        _connectionString = connectionString.Value;
        _connectionManager = connectionManager;
    }

    public async Task<IEnumerable<Entity>> GetAll()
    {
        var settings = MongoClientSettings.FromConnectionString(_connectionString.Default);
        var client = new MongoClient(settings);
        var db = client.GetDatabase(Database);
        var collection = db.GetCollection<Entity>(Calendar);

        var a = await collection.FindAsync(_ => true);

        return a.ToList();
    }

    public async Task<Result<PlayerEntity, Exception>> GetPlayer(LoginRequest request)
    {
        var sdf = new PlayerEntity { Username = "sdf", Password = "sdf" };
        var a = await Result.Try(
            async () =>
            {
                var test = await _connectionManager.ExecuteAsync(async db =>
                {
                    var collection = db.GetCollection<PlayerEntity>(Login);

                    var player = await collection.FindAsync(x =>
                        x.Username == request.Username && x.Password == request.Password
                    );
                    return await player.FirstOrDefaultAsync() ?? sdf; //TODO: try to return exception
                });

                return test.Match(player => player, e => sdf); //TODO: Add custom exception;
            },
            e => e
        );
        return a;
    }

    public async Task<Result<bool, Exception>> RegisterPlayer(LoginRequest request)
    {
        return await _connectionManager.ExecuteAsync(db =>
        {
            var collection = db.GetCollection<PlayerEntity>(Login);

            var a = collection.Find(x => x.Username == request.Username).FirstOrDefaultAsync();
            if (a == null)
            {
                //TODO: Add custom exception (User already exists)
                return Task.FromResult(false);
            }
            collection.InsertOne(
                new PlayerEntity { Username = request.Username, Password = request.Password }
            );
            return Task.FromResult(true);
        });
    }
}

public sealed class Entity
{
    [BsonId]
    public ObjectId Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public FreeTime FreeTime { get; set; } = new();
}
