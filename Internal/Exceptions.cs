namespace CalendarApi.Internal;

public sealed class DatabaseException : Exception
{
    private DatabaseException(string message, Exception e)
        : base(message) { }

    public static DatabaseException New(Exception e) => new("Error in db", e);
}

public sealed class UserAlreadyExists : Exception
{
    private UserAlreadyExists(string message) : base(message) { }

    public static UserAlreadyExists New(string name) => new($"User already exists: {name}");
}

public sealed class InvalidCredentialException : Exception
{
    private InvalidCredentialException(string message) : base(message) { }

    public static InvalidCredentialException New() => new("Invalid username or password");
}

public sealed class ValidationException : Exception
{
    private ValidationException(string message) : base(message) { }

    public static ValidationException New(string error) => new(error);
}
