using gitViwe.Shared;
using Microsoft.AspNetCore.Mvc;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<IRedisDistributedCache, RedisDistributedCache>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? "localhost:6379";
    options.InstanceName = "redis_demo";
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/url/shorten", async ([FromServices] IRedisDistributedCache redis, [FromServices] IHttpContextAccessor context, string url) =>
{
    string key = Conversion.RandomString(7);
    string requestHost = context.HttpContext.Request.Host.Value;
    string protocolScheme = context.HttpContext.Request.Scheme;
    string shortUrl = $"{protocolScheme}{Uri.SchemeDelimiter}{requestHost}/url/get/{key}";

    await redis.SetAsync(key, value: url);

    return Results.Ok(new UrlShortenResponse(shortUrl, key, DateTime.UtcNow.AddMinutes(5)));
})
.WithName("ShortedUrl")
.WithOpenApi();

app.MapGet("/url/get/{key}", async ([FromServices] IRedisDistributedCache redis, string key) =>
{
    string value = await redis.GetAsync(key) ?? string.Empty;
    return value;
})
.WithName("GetUrl")
.WithOpenApi();

app.Run();

public record UrlShortenResponse(string ShortUri, string key, DateTime ValidUntil);