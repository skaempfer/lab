using System.Text.Json;

using Microsoft.Extensions.Options;

using MyWebApp;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.Configure<MyServiceSettings>(configuration.GetSection("MyService"));

var app = builder.Build();

app.MapGet("/", (IOptions<MyServiceSettings> options) =>
{
	var stringifiedSettings = JsonSerializer.Serialize(options.Value, JsonSerializerOptions.Web);
	
	return Results.Text(stringifiedSettings, "application/json");
});

app.Run();