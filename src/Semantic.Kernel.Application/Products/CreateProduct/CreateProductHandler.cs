using MediatR;
using Semantic.Kernel.Domain._Shared;
using Semantic.Kernel.Domain.Products;
using Semantic.Kernel.Domain.Recommendations;

namespace Semantic.Kernel.Application.Products.CreateProduct;


public class CreateProductHandler : IRequestHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IRecommendationRepository _recommendationRepository;
    private readonly IEmbeddingService _embeddingService;

    public CreateProductHandler(
        IProductRepository productRepository,
        IRecommendationRepository recommendationRepository,
        IEmbeddingService embeddingService)
    {
        _productRepository        = productRepository;
        _recommendationRepository = recommendationRepository;
        _embeddingService         = embeddingService;
    }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await AddProductAsync(request);
        await AddRecommendationAsync(request);
        
        await _productRepository.SaveChangesAsync();
    }
    
    private async Task AddProductAsync(CreateProductCommand request)
    {
        var product = new Product
        {
            Title       = request.Product.Title,
            Category    = request.Product.Category,
            Summary     = request.Product.Summary,
            Description = request.Product.Description
        };

        await _productRepository.AddAsync(product);
    }

    private async Task AddRecommendationAsync(CreateProductCommand request)
    {
        var embedding = await _embeddingService.GenerateEmbeddingAsync(request.Product.Category);
        var recommendation = new Recommendation
        {
            Title     = request.Product.Title,
            Category  = request.Product.Category,
            Embedding = embedding
        };

        await _recommendationRepository.AddAsync(recommendation);
    }
}