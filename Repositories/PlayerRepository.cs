using CSharpFunctionalExtensions;

public interface IPlayerRepository
{
    Task<Result<PlayerResponse, Exception>> Get(int id);
    Task<Result<IEnumerable<PlayerResponse>, Exception>> GetAll();
}

public sealed class PlayerRepository : IPlayerRepository
{
    public Task<Result<PlayerResponse, Exception>> Get(int id)
    {
        return Task.FromResult(Result.Success<PlayerResponse, Exception>(new PlayerResponse()));
    }

    public Task<Result<IEnumerable<PlayerResponse>, Exception>> GetAll()
    {
        return Task.FromResult(Result.Success<IEnumerable<PlayerResponse>, Exception>([]));
    }
}
