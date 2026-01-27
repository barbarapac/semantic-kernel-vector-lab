using System.ComponentModel;
using ModelContextProtocol.Server;
using Semantic.Kernel.MCP.Service.Clients;
using Semantic.Kernel.MCP.Service.Dtos;

namespace Semantic.Kernel.MCP.Service.Tools;

[McpServerToolType]
public class SemanticKernelTools
{
    private readonly SemanticKernelApiClient _client;

    public SemanticKernelTools(SemanticKernelApiClient client)
    {
        _client = client;
    }

    [McpServerTool]
    [Description("Registra um novo produto na Semantic Kernel Web API.")]
    public async Task<string> CreateProductAsync(
        [Description("Título do produto.")] string title,
        [Description("Categoria do produto.")] string category,
        [Description("Resumo breve do produto.")] string summary,
        [Description("Descrição completa do produto.")] string description,
        CancellationToken cancellationToken = default)
    {
        var dto = new CreateProductDto
        {
            Title = title,
            Category = category,
            Summary = summary,
            Description = description
        };

        await _client.CreateProductAsync(dto, cancellationToken);
        return "Produto criado com sucesso.";
    }

    [McpServerTool]
    [Description("Busca recomendações a partir de um prompt.")]
    public async Task<IEnumerable<RecommendationDto>> SearchRecommendationsAsync(
        [Description("Prompt utilizado para buscar recomendações."), DefaultValue("Sugira categorias de tecnologia")] string prompt,
        CancellationToken cancellationToken = default)
    {
        var recommendations = await _client.SearchRecommendationsAsync(prompt, cancellationToken);
        return recommendations;
    }

    // [McpServerTool]
    // [Description("Dispara a rotina de seed para recomendações.")]
    // public async Task<string> SeedRecommendationsAsync(CancellationToken cancellationToken = default)
    // {
    //     await _client.SeedRecommendationsAsync(cancellationToken);
    //     return "Seed de recomendações executado com sucesso.";
    // }
}