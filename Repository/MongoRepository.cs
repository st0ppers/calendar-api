using CalendarApi.Contracts;
using CalendarApi.Contracts.Entities;
using CalendarApi.Contracts.Response;
using CalendarApi.Internal;
using CSharpFunctionalExtensions;
using MongoDB.Driver;
using static CalendarApi.Internal.Mappings;

namespace CalendarApi.Repository;

public interface IMongoRepository
{
    public Task<Result<PlayerResponse, Exception>> GetPlayer(PlayerEntity entity);
    public Task<Result<PlayerResponse, Exception>> RegisterPlayer(PlayerEntity entity);
    public Task<Result<PlayerEntity, Exception>> CheckIfUserExists(PlayerEntity entity);
    public Task<Result<IEnumerable<PlayerResponse>, Exception>> GetAll(int groupId);
    public Task<Result<long, Exception>> UpdateFreeTime(UpdateFreeTimeEntity entity);
}

public sealed class MongoRepository(ConnectionManager connectionManager) : IMongoRepository
{
    public async Task<Result<PlayerResponse, Exception>> GetPlayer(PlayerEntity request) =>
        await connectionManager
            .ExecuteCollectionAsync<PlayerEntity, PlayerEntity>(async coll =>
            {
                var player = await coll.FindAsync(x => x.Username == request.Username && x.Password == request.Password);
                return await player.FirstAsync();
            })
            .Map(ToResponse);//TODO Add to service because repo is not responsible for mapping

    public async Task<Result<PlayerResponse, Exception>> RegisterPlayer(PlayerEntity entity) =>
        await connectionManager
            .ExecuteCollectionAsync<PlayerEntity, PlayerEntity>(async coll =>
            {
                await coll.InsertOneAsync(entity);
                return entity;
            })
            .Map(ToResponse);

    public async Task<Result<PlayerEntity, Exception>> CheckIfUserExists(PlayerEntity entity) =>
        await connectionManager
            .ExecuteCollectionAsync<bool, PlayerEntity>(async coll =>
            {
                var result = await coll.FindAsync(x => x.Username == entity.Username);
                return await result.AnyAsync();
            })
            .Bind(x => x ? UserAlreadyExists.New(entity.Username) : Result.Success<PlayerEntity, Exception>(entity));

    public async Task<Result<IEnumerable<PlayerResponse>, Exception>> GetAll(int groupId) =>
        await connectionManager
            .ExecuteCollectionAsync<IEnumerable<PlayerEntity>, PlayerEntity>(async coll =>
            {
                var result = await coll.FindAsync(x => x.GroupId == groupId);
                return await result.ToListAsync();
            })
            .Map(e => e.Select(ToResponse));

    public async Task<Result<long, Exception>> UpdateFreeTime(UpdateFreeTimeEntity entity) =>
        await connectionManager
            .ExecuteCollectionAsync<long, PlayerEntity>(async coll =>
            {
                var freeTime = new FreeTime { From = entity.From, To = entity.To };
                var result = await coll.UpdateOneAsync(x => x.Id == entity.PlayerId, Builders<PlayerEntity>.Update.Set(x => x.FreeTime, freeTime));
                return result.ModifiedCount;
            });
}

