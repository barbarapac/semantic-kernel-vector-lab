using MediatR;
using Semantic.Kernel.Application.Recommendations.SeedData;

namespace Semantic.Kernel.WebApi.Recommendations;

public static class SeedDataEndpoints
{
    public static void MapSeedDataEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("v1/seed", async (IMediator mediator) =>
        {
            await mediator.Send(new SeedDataCommand());
            return Results.Ok(new { message = "OK!" });
        });
    }
}