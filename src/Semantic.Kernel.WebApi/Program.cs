using Microsoft.EntityFrameworkCore;
using OllamaSharp;
using Semantic.Kernel.Application.Products.CreateProduct;
using Semantic.Kernel.Domain._Shared;
using Semantic.Kernel.Domain.Products;
using Semantic.Kernel.Domain.Recommendations;
using Semantic.Kernel.Infrastructure.Data;
using Semantic.Kernel.Infrastructure.Repositories;
using Semantic.Kernel.Infrastructure.Services;
using Semantic.Kernel.WebApi.Products;
using Semantic.Kernel.WebApi.Recommendations;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(connectionString, p => p.UseVector());
});

builder.Services.AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Debug));

builder.Services.AddTransient<OllamaApiClient>(x => new OllamaApiClient(
    uriString: "http://localhost:11434",
    defaultModel: "mxbai-embed-large"
));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();
builder.Services.AddScoped<IEmbeddingService, OllamaEmbeddingService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

var app = builder.Build();

app.MapProductEndpoints();
app.MapSeedDataEndpoints();
app.MapSearchRecommendationsEndpoints();

app.Run();