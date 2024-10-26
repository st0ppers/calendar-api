using System.Text;
using CSharpFunctionalExtensions;

namespace CalendarApi.Internal;

public sealed class Validator<T>
{
    public T Value { get; }
    private IEnumerable<string> Errors { get; set; }

    private Validator(T value)
    {
        Value = value;
        Errors = Enumerable.Empty<string>();
    }

    public static Validator<T> Instance(T t) => new(t);

    public void AddError(string message) => Errors = Errors.Append(message);
    public bool HasErrors() => Errors.Any();

    public string GetErrorMessage()
    {
        var sb = new StringBuilder();
        foreach (var error in Errors)
        {
            sb.AppendLine(error);
        }

        return sb.ToString();
    }
}

public static class Validations
{
    public static Validator<T> Validate<T>(this Validator<T> t, Func<T, bool> predicate, string message)
    {
        if (predicate(t.Value))
        {
            t.AddError(message);
        }

        return t;
    }

    public static Result<T, Exception> ToResult<T>(this Validator<T> t) 
    {
        return t.HasErrors()
            ? Result.Failure<T,Exception>(ValidationException.New(t.GetErrorMessage()))
            : Result.Success<T, Exception>(t.Value);
    }
}
