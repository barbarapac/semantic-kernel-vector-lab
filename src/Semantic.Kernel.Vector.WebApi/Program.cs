using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using Semantic.Kernel.Vector.WebApi.Data;
using Semantic.Kernel.Vector.WebApi.Dtos;
using Semantic.Kernel.Vector.WebApi.Models;

var builder          = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(connectionString, p => p.UseVector());
});


builder.Services.AddTransient<OllamaApiClient>(x => new OllamaApiClient(
    uriString: "http://localhost:11434",
    defaultModel: "mxbai-embed-large"
));

var app = builder.Build();

app.MapGet("v1/seed", async (AppDbContext dbContext, OllamaApiClient ollama) =>
{
    var products = await dbContext.Products.AsNoTracking().ToListAsync();

    foreach (var product in products)
    {
        var service = ollama.AsTextEmbeddingGenerationService();
        var embeddings = await service.GenerateEmbeddingAsync(product.Category);

        var recomendation = new Recomendation()
        {
            Title     = product.Title,
            Category  = product.Category,
            Embedding = new Vector(embeddings)
        };

        await dbContext.Recomendations.AddAsync(recomendation);
        await dbContext.SaveChangesAsync();
    }

    return Results.Ok(new { message = "OK!" });;
});

app.MapPost("/v1/products", async (
    CreateProductDto dto,
    AppDbContext db,
    OllamaApiClient ollamaClient) =>
{
    var product = new Product()
    {
        Title = dto.Title,
        Summary = dto.Summary,
        Category = dto.Category,
        Description = dto.Description
    };
    
    await db.Products.AddAsync(product);
    
    var service = ollamaClient.AsTextEmbeddingGenerationService();
    var embeddings = await service.GenerateEmbeddingAsync(dto.Category);

    var recomendation = new Recomendation
    {
        Title = dto.Title,
        Category = dto.Category,
        Embedding = new Vector(embeddings)
    };

    await db.Recomendations.AddAsync(recomendation);
    await db.SaveChangesAsync();
    
    return Results.Created();
});

app.MapPost("/v1/prompt", async (
    QuestionDto model,
    AppDbContext db,
    OllamaApiClient ollamaClient) =>
{
    var service    = ollamaClient.AsTextEmbeddingGenerationService();
    var embeddings = await service.GenerateEmbeddingAsync(model.Prompt);

    var recomendations = await db.Recomendations
        .AsNoTracking()
        .OrderBy(d => d.Embedding.CosineDistance(new Vector(embeddings.ToArray())))
        .Take(3)
        .Select(x => new
        {
            x.Title, x.Category
        })
        .ToListAsync();

    return Results.Ok(recomendations);
});

app.Run();