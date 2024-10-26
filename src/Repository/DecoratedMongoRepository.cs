using App.Metrics;
using CalendarApi.Contracts.Entities;
using CalendarApi.Contracts.Response;
using CSharpFunctionalExtensions;
using Serilog;
using static CalendarApi.Internal.MetricsOptions;

namespace CalendarApi.Repository;

public sealed class DecoratedMongoRepository(IMongoRepository repository) : IMongoRepository
{
    public Task<Result<PlayerResponse, Exception>> GetPlayer(PlayerEntity entity)
    {
        using var _ = Metrics.Instance.Measure.Timer.Time(ExecutionLatency, new MetricTags("MethodName", nameof(GetPlayer)));
        return repository.GetPlayer(entity)
            .Tap(x => Log.Debug("Player {Username} logged in", x.Username));
    }

    public Task<Result<PlayerResponse, Exception>> RegisterPlayer(PlayerEntity entity)
    {
        using var _ = Metrics.Instance.Measure.Timer.Time(ExecutionLatency, new MetricTags("MethodName", nameof(RegisterPlayer)));
        return repository.RegisterPlayer(entity)
            .Tap(x => Log.Debug("Player {Username} registered", x.Username));
    }

    public Task<Result<PlayerEntity, Exception>> CheckIfUserExists(PlayerEntity entity)
    {
        using var _ = Metrics.Instance.Measure.Timer.Time(ExecutionLatency, new MetricTags("MethodName", nameof(CheckIfUserExists)));
        return repository.CheckIfUserExists(entity)
            .Tap(_ => Log.Debug("Checking if {Username} exists", entity.Username));
    }

    public Task<Result<IEnumerable<PlayerResponse>, Exception>> GetAll(int groupId)
    {
        using var _ = Metrics.Instance.Measure.Timer.Time(ExecutionLatency, new MetricTags("MethodName", nameof(GetAll)));
        return repository.GetAll(groupId)
            .Tap(_ => Log.Debug("Getting all players for GroupId: {Id}", groupId));
    }

    public Task<Result<long, Exception>> UpdateFreeTime(UpdateFreeTimeEntity entity)
    {
        using var _ = Metrics.Instance.Measure.Timer.Time(ExecutionLatency, new MetricTags("MethodName", nameof(UpdateFreeTime)));
        return repository.UpdateFreeTime(entity)
            .Tap(_ => Log.Debug("Updating free time for PlayerId: {Id}", entity.PlayerId));
        ;
    }
}