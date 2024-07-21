public static class DependencyInjection
{
    public static IServiceCollection AddRrepository(this IServiceCollection services)
    {
        services.AddSingleton<IPlayerRepository, PlayerRepository>();
        return services;
    }
}
