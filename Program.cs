using CalendarApi;
using CalendarApi.Internal;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(builder.Configuration).AddRrepository().AddConfiguration(builder.Configuration).AddAuth(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //enable cors policy for every ednpoint
    app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(_ => true));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Log.Logger.Information("Application started");
Log.Logger.Information("ConnString: {0}", app.Configuration.GetConnectionString("Default"));
Log.Logger.Information("Key: {0}", app.Configuration.GetRequiredSection(JwtOptions.Section).GetSection("SigningKey").Value);

;
app.Run();
