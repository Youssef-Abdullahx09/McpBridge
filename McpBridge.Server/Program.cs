using McpBridge.Server.Domain.Interfaces;
using McpBridge.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        options.Stateless = true;
    })
    .WithToolsFromAssembly();

builder.Services.AddSingleton<
    IBookmarkRepository,
    InMemoryBookmarkRepository>();

var app = builder.Build();

app.MapMcp("/mcp");

app.MapGet("/healthx", () => "Healthy");

await app.RunAsync(CancellationToken.None);