namespace Semantic.Kernel.Vector.WebApi.Models;

public class Recomendation
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Pgvector.Vector Embedding { get; set; } = null!;
}