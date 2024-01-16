var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCORS", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();

    });
    options.AddPolicy("ProdCORS", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("https://{ProductionSiteName.com}")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCORS");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // if the app is in development mode we don't have an SSL. HTTPS will cause issues when testing. We want to only implement this in production. in testing we will use http in the routes
    app.UseHttpsRedirection();
    app.UseCors("ProdCORS");
}

app.MapControllers();

// app.MapGet("/weatherforecast", () =>
// {
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.Run();

