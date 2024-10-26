using CalendarApi;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddSerilog()
    .AddRepository()
    .AddConfiguration(builder.Configuration)
    .AddAuth(builder.Configuration);
builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Host.ConfigureMetrics();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true));
}

app.MapHealthChecks("/healthz");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
