using MediatR;
using Semantic.Kernel.Application._Shared;
using Semantic.Kernel.Application.Products.CreateProduct;

namespace Semantic.Kernel.WebApi.Products;

public static class CreateProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/products", async (CreateProductDto dto, IMediator mediator) =>
        {
            await mediator.Send(new CreateProductCommand(dto));
            return Results.Created();
        });
    }
}