namespace CalendarApi.Internal;

public static class Constants
{
    public const string App = "CalendarApi";

    public static class Database
    {
        public const string Name = "DungeonsAndDragons";
        public const string Login = "Login";
    }

    public static class Metrics
    {
        public const string RequestCount = "RequestCount";
        public const string DatabaseConnectionsCount = "DatabaseConnectionsCount";
        public const string ExecutionLatency = "ExecutionLatency";
        public const string FailedRequests = "FailedRequests";
    }
}