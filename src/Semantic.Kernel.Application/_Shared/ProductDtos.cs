namespace Semantic.Kernel.Application._Shared;

public record CreateProductDto(string Title, string Category, string Summary, string Description);

public record SearchDto(string Prompt);

public record RecommendationDto(string Title, string Category);