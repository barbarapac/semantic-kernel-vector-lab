namespace Semantic.Kernel.Domain.Recommendations;

public class Recommendation
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public float[] Embedding { get; set; } = [];
}