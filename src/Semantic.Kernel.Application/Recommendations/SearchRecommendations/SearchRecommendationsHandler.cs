using MediatR;
using Semantic.Kernel.Application._Shared;
using Semantic.Kernel.Domain._Shared;
using Semantic.Kernel.Domain.Recommendations;

namespace Semantic.Kernel.Application.Recommendations.SearchRecommendations;

public class SearchRecommendationsHandler : IRequestHandler<SearchRecommendationsQuery, IEnumerable<RecommendationDto>>
{
    private readonly IRecommendationRepository _recommendationRepository;
    private readonly IEmbeddingService _embeddingService;

    public SearchRecommendationsHandler(
        IRecommendationRepository recommendationRepository,
        IEmbeddingService embeddingService)
    {
        _recommendationRepository = recommendationRepository;
        _embeddingService         = embeddingService;
    }

    public async Task<IEnumerable<RecommendationDto>> Handle(SearchRecommendationsQuery request, CancellationToken cancellationToken)
    {
        var embedding       = await _embeddingService.GenerateEmbeddingAsync(request.Prompt);
        var recommendations = await _recommendationRepository.GetSimilarAsync(embedding);

        return recommendations.Select(r => new RecommendationDto(r.Title, r.Category));
    }
}