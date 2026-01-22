using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Semantic.Kernel.Domain._Shared;

namespace Semantic.Kernel.Infrastructure.Services;

public class OllamaEmbeddingService : IEmbeddingService
{
    private readonly OllamaApiClient _ollamaClient;

    public OllamaEmbeddingService(OllamaApiClient ollamaClient)
    {
        _ollamaClient = ollamaClient;
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var service    = _ollamaClient.AsTextEmbeddingGenerationService();
        var embeddings = await service.GenerateEmbeddingAsync(text);
        
        return embeddings.ToArray();
    }
}