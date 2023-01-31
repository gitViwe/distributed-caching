using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "redis_demo";
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/set-value", async ([FromServices] IDistributedCache redis) =>
{

    await redis.SetAsync("my-key", Encoding.UTF8.GetBytes("my-value"), CancellationToken.None);

    var response = await redis.GetAsync("my-key", CancellationToken.None);

    return Encoding.UTF8.GetString(response!);
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
