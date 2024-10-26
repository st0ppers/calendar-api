using System.Text;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using CalendarApi.Contracts.Configurations;
using CalendarApi.Internal;
using CalendarApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace CalendarApi;

public static class DependencyInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services
            .AddSingleton<ConnectionManager>()
            .AddSingleton<IMongoRepository, MongoRepository>()
            .Decorate<IMongoRepository, DecoratedMongoRepository>();

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration config) =>
        services
            .Configure<ConnectionString>(config.GetSection(ConnectionString.Section))
            .Configure<OpenTelemetryOptions>(config.GetSection(OpenTelemetryOptions.Section))
            .Configure<JwtOptions>(config.GetSection(JwtOptions.Section));

    public static void AddAuth(this IServiceCollection services, IConfiguration config)
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
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });
    }

    public static IServiceCollection AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        var openTelemetryOptions = builder.Configuration.GetRequiredSection(OpenTelemetryOptions.Section).Get<OpenTelemetryOptions>();
        Log.Logger = new LoggerConfiguration().ReadFrom
            .Configuration(builder.Configuration)
            .WriteTo.OpenTelemetry(x =>
            {
                x.Endpoint = openTelemetryOptions!.Endpoint;
                x.Protocol = OtlpProtocol.HttpProtobuf;
                x.Headers = new Dictionary<string, string>
                {
                    ["X-Seq-ApiKey"] = openTelemetryOptions.ApiKey
                };
                x.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = Constants.App,
                    ["environment"] = builder.Environment.EnvironmentName
                };
            })
            .CreateLogger();
        return builder.Services.AddLogging(b => b.AddSerilog());
    }

    public static void ConfigureMetrics(this ConfigureHostBuilder host)
    {
        var metrics = AppMetrics.CreateDefaultBuilder()
            .OutputMetrics.AsPrometheusPlainText()
            .OutputMetrics.AsPrometheusProtobuf()
            .Build();

        host.ConfigureMetrics(metrics)
            .UseMetrics(op =>
            {
                op.EndpointOptions = endpointOptions =>
                {
                    endpointOptions.MetricsTextEndpointOutputFormatter = Metrics.Instance.OutputMetricsFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First();
                    endpointOptions.MetricsEndpointOutputFormatter = Metrics.Instance.OutputMetricsFormatters.OfType<MetricsPrometheusProtobufOutputFormatter>().First();
                };
            });
    }
}
