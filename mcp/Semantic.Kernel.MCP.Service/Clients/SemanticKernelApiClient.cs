using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Semantic.Kernel.MCP.Service.Dtos;

namespace Semantic.Kernel.MCP.Service.Clients;

public class SemanticKernelApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public SemanticKernelApiClient(HttpClient httpClient)
    {
        _httpClient  = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync("/v1/products", dto, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task<IEnumerable<RecommendationDto>> SearchRecommendationsAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var payload = new SearchDto { Prompt = prompt };

        using var response = await _httpClient.PostAsJsonAsync("/v1/prompt", payload, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var recommendations = await response.Content.ReadFromJsonAsync<IEnumerable<RecommendationDto>>(_jsonOptions, cancellationToken);
        return recommendations ?? Enumerable.Empty<RecommendationDto>();
    }

    public async Task SeedRecommendationsAsync(CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync("/v1/seed", cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        var message = new StringBuilder()
            .AppendLine($"Request failed with status code {(int)response.StatusCode} ({response.ReasonPhrase}).")
            .Append(body)
            .ToString();

        throw new HttpRequestException(message, null, response.StatusCode);
    }
}