using CalendarApi.Contracts;
using CalendarApi.Contracts.Entities;
using CalendarApi.Contracts.Response;
using CalendarApi.Internal;
using CSharpFunctionalExtensions;
using MongoDB.Driver;
using static CalendarApi.Internal.Constants.Database;

namespace CalendarApi.Repository;

public interface IMongoRepository
{
    public Task<Result<PlayerResponse, Exception>> GetPlayer(PlayerEntity entity);
    public Task<Result<PlayerResponse, Exception>> RegisterPlayer(PlayerEntity entity);
    public Task<Result<bool, Exception>> CheckIfUserExists(PlayerEntity entity);
    public Task<Result<IEnumerable<PlayerResponse>, Exception>> GetAll(int groupId);
    public Task<Result<long, Exception>> UpdateFreeTime(UpdateFreeTimeEntity entity);
}

public sealed class MongoRepository(ConnectionManager connectionManager) : IMongoRepository
{
    public async Task<Result<PlayerResponse, Exception>> GetPlayer(PlayerEntity request) =>
        await connectionManager.ExecuteAsync(async db =>
            {
                var player = await db.GetCollection<PlayerEntity>(Login)
                    .FindAsync(x => x.Username == request.Username && x.Password == request.Password);
                return await player.FirstAsync();
            })
            .Map(x => x.ToResponse());

    public async Task<Result<PlayerResponse, Exception>> RegisterPlayer(PlayerEntity entity) =>
        await connectionManager.ExecuteCollectionAsync<PlayerEntity, PlayerEntity>(async coll =>
            {
                await coll.InsertOneAsync(entity);
                return entity;
            }, Login)
            .Map(x => x.ToResponse());

    public async Task<Result<bool, Exception>> CheckIfUserExists(PlayerEntity request) =>
        await connectionManager.ExecuteCollectionAsync<bool, PlayerEntity>(async coll =>
            {
                var result = await coll.FindAsync(x => x.Username == request.Username);
                return await result.AnyAsync();
            }, Login)
            .Bind(x => x ? UserAlreadyExists.New(request.Username) : Result.Success<bool, Exception>(x));

    public async Task<Result<IEnumerable<PlayerResponse>, Exception>> GetAll(int groupId) =>
        await connectionManager.ExecuteCollectionAsync<IEnumerable<PlayerEntity>, PlayerEntity>(async coll =>
            {
                var result = await coll.FindAsync(x => x.GroupId == groupId);
                return await result.ToListAsync();
            }, Login)
            .Map(e => e.Select(x => x.ToResponse()));

    public async Task<Result<long, Exception>> UpdateFreeTime(UpdateFreeTimeEntity entity) =>
        await connectionManager.ExecuteCollectionAsync<long, PlayerEntity>(async coll =>
        {
            var freeTime = new FreeTime { From = entity.From, To = entity.To };
            var result = await coll.UpdateOneAsync(
                x => x.Id == entity.PlayerId,
                Builders<PlayerEntity>.Update.Set(x => x.FreeTime, freeTime));
            return result.ModifiedCount;
        }, Login);
}