using gitViwe.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
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

app.MapGet("/url/shorten", async ([FromServices] IDistributedCache redis, [FromServices] IHttpContextAccessor context, string url) =>
{
    string shortUrl = context.HttpContext!.Request.Host.Value + "/" + Conversion.RandomString(7);

    await redis.SetAsync(shortUrl, Encoding.UTF8.GetBytes(url), CancellationToken.None);

    return shortUrl;
})
.WithName("ShortedUrl")
.WithOpenApi();

app.MapGet("/url/get-original", async ([FromServices] IDistributedCache redis, string shortUrl) =>
{
    var response = await redis.GetAsync(shortUrl, CancellationToken.None);

    return Encoding.UTF8.GetString(response!);
})
.WithName("GetUrl")
.WithOpenApi();

app.Run();
