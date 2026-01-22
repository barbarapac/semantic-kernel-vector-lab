using MediatR;
using Semantic.Kernel.Application._Shared;
using Semantic.Kernel.Application.Recommendations.SearchRecommendations;

namespace Semantic.Kernel.WebApi.Recommendations;

public static class SearchRecommendationsEndpoints
{
    public static void MapSearchRecommendationsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/prompt", async (SearchDto dto, IMediator mediator) =>
        {
            var recommendations = await mediator.Send(new SearchRecommendationsQuery(dto.Prompt));
            return Results.Ok(recommendations);
        });
    }
}