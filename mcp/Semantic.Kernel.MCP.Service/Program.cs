using ModelContextProtocol.Protocol;
using Semantic.Kernel.MCP.Service.Clients;
using Semantic.Kernel.MCP.Service.Tools;

var builder = WebApplication.CreateBuilder(args);

// Configure URLs
builder.WebHost.UseUrls("http://localhost:5010");

var serverInfo = new Implementation { Name = "Semantic.Kernel.MCP.Service", Version = "1.0.0" };
builder.Services
    .AddMcpServer(mcp =>
    {
        mcp.ServerInfo = serverInfo;
    })
    .WithHttpTransport()
    .WithToolsFromAssembly(typeof(SemanticKernelTools).Assembly);

builder.Services.AddHttpClient<SemanticKernelApiClient>(client =>
{
    var baseAddress = Environment.GetEnvironmentVariable("API_BASE_ADDRESS");
    if (!string.IsNullOrEmpty(baseAddress))
        client.BaseAddress = new Uri(baseAddress);
    else
        client.BaseAddress = new Uri("http://localhost:5042");
});

var app = builder.Build();

app.MapMcp();

await app.RunAsync();