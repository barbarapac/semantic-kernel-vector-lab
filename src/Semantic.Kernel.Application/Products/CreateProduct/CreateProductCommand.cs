using MediatR;
using Semantic.Kernel.Application._Shared;

namespace Semantic.Kernel.Application.Products.CreateProduct;

public record CreateProductCommand(CreateProductDto Product) : IRequest;
