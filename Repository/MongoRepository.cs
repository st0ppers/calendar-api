using CalendarApi.Contracts.Entities;
using CalendarApi.Contracts.Response;
using CalendarApi.Internal;
using CSharpFunctionalExtensions;
using MongoDB.Driver;
using static CalendarApi.Internal.Constants.Database;

namespace CalendarApi.Repository;

public interface IMongoRepository
{
    public Task<Result<bool, Exception>> LoginPlayer(LoginEntity request);
    public Task<Result<PlayerResponse, Exception>> GetPlayer(LoginEntity request);
    public Task<Result<PlayerResponse, Exception>> RegisterPlayer(PlayerEntity entity);
    public Task<Result<bool, Exception>> CheckIfUserExists(LoginEntity request);
}

public sealed class MongoRepository(ConnectionManager connectionManager) : IMongoRepository
{
    public async Task<Result<bool, Exception>> LoginPlayer(LoginEntity request)
    {
        return await connectionManager.ExecuteCollectionAsync<bool, PlayerEntity>(async coll =>
            {
                var result = await coll.FindAsync(x => x.Username == request.Username && x.Password == request.Password);
                return await result.AnyAsync();
            }, Login)
            .Bind(x => x ? Result.Success<bool, Exception>(x) : InvalidCredentialException.New());
    }

    public async Task<Result<PlayerResponse, Exception>> GetPlayer(LoginEntity request)
    {
        return await connectionManager.ExecuteAsync(async db =>
            {
                var player = await db.GetCollection<PlayerEntity>(Login)
                    .FindAsync(x => x.Username == request.Username && x.Password == request.Password);

                return await player.FirstAsync();
            })
            .Map(x => x.ToResponse());
    }

    public async Task<Result<PlayerResponse, Exception>> RegisterPlayer(PlayerEntity entity)
    {
        return await connectionManager.ExecuteCollectionAsync<PlayerEntity, PlayerEntity>(async coll =>
            {
                await coll.InsertOneAsync(entity);
                return entity;
            }, Login)
            .Map(x => x.ToResponse());
    }

    public async Task<Result<bool, Exception>> CheckIfUserExists(LoginEntity request)
    {
        return await connectionManager.ExecuteCollectionAsync<bool, PlayerEntity>(async coll =>
            {
                var result = await coll.FindAsync(x => x.Username == request.Username);
                return await result.AnyAsync();
            }, Login)
            .Bind(x => x ? UserAlreadyExists.New(request.Username) : Result.Success<bool, Exception>(x));
    }
}