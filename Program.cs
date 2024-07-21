var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRrepository();
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
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
