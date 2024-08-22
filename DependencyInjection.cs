using System.Text;
using CalendarApi.Internal;
using CalendarApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace CalendarApi;

public static class DependencyInjection
{
    public static IServiceCollection AddRrepository(this IServiceCollection services)
    {
        services.AddSingleton<ConnectionManager>();
        services.AddSingleton<IMongoRepository, MongoRepository>();
        return services;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ConnectionString>(config.GetSection(ConnectionString.Section));
        services.Configure<JwtOptions>(config.GetSection(JwtOptions.Section));

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        var jwtOptions = config.GetRequiredSection(JwtOptions.Section).Get<JwtOptions>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtOptions!.SigningKey);

                opts.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
                };
            });

        return services;
    }

    public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration config)
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

        return services.AddLogging(builder => builder.AddSerilog());
    }
}
