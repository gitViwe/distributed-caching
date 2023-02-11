using gitViwe.Shared;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.RegisterRedisCache(builder.Configuration);
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

app.MapPost("/url/shorten", async ([FromServices] IRedisDistributedCache redis, [FromServices] IHttpContextAccessor context, [FromBody] UrlShortenRequest request) =>
{
    string key = Conversion.RandomString(7);
    string requestHost = context.HttpContext.Request.Host.Value;
    string protocolScheme = context.HttpContext.Request.Scheme;
    string shortUrl = $"{protocolScheme}{Uri.SchemeDelimiter}{requestHost}/url/get/{key}";

    await redis.SetAsync(key, value: request.Uri, absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(request.MinutesUntilExpiry));

    return Results.Ok(new UrlShortenResponse(shortUrl, key, DateTime.UtcNow.AddMinutes(request.MinutesUntilExpiry)));
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

public record UrlShortenRequest(string Uri, int MinutesUntilExpiry);
public record UrlShortenResponse(string ShortUri, string key, DateTime ValidUntil);