namespace Semantic.Kernel.Domain._Shared;

public interface IEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text);
}