using MediatR;
using Semantic.Kernel.Application._Shared;

namespace Semantic.Kernel.Application.Recommendations.SearchRecommendations;

public record SearchRecommendationsQuery(string Prompt) : IRequest<IEnumerable<RecommendationDto>>;

