using System.Runtime.CompilerServices;
using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Timer;
using CSharpFunctionalExtensions;

namespace CalendarApi.Internal;

public static class MetricsOptions
{
    private static readonly CounterOptions RequestCount = new() { Name = Constants.Metrics.RequestCount, Context = Constants.App };
    private static readonly CounterOptions FailedRequests = new() { Name = Constants.Metrics.FailedRequests, Context = Constants.App };
    private static readonly CounterOptions DatabaseConnectionsCount = new() { Name = Constants.Metrics.DatabaseConnectionsCount, Context = Constants.App };
    public static readonly TimerOptions ExecutionLatency = new() { Name = Constants.Metrics.ExecutionLatency, Context = Constants.App };

    public static Task<Result<T, Exception>> CountRequest<T>(this Task<Result<T, Exception>> task, [CallerMemberName] string name = "") =>
        task.Tap(() => Metrics.Instance.Measure.Counter.Increment(RequestCount, new MetricTags("MethodName", name)));

    public static Task<Result<T, Exception>> CountFailedRequest<T>(this Task<Result<T, Exception>> task, [CallerMemberName] string name = "") =>
        task.TapError(() => Metrics.Instance.Measure.Counter.Increment(FailedRequests, new MetricTags("MethodName", name)));

    public static void CountDatabaseConnections() =>
        Metrics.Instance.Measure.Counter.Increment(DatabaseConnectionsCount);
}