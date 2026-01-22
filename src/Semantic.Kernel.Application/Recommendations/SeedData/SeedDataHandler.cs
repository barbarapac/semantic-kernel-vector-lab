using MediatR;
using Semantic.Kernel.Domain._Shared;
using Semantic.Kernel.Domain.Products;
using Semantic.Kernel.Domain.Recommendations;

namespace Semantic.Kernel.Application.Recommendations.SeedData;


public class SeedDataHandler : IRequestHandler<SeedData.SeedDataCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IRecommendationRepository _recommendationRepository;
    private readonly IEmbeddingService _embeddingService;

    public SeedDataHandler(
        IProductRepository productRepository,
        IRecommendationRepository recommendationRepository,
        IEmbeddingService embeddingService)
    {
        _productRepository        = productRepository;
        _recommendationRepository = recommendationRepository;
        _embeddingService         = embeddingService;
    }

    public async Task Handle(SeedDataCommand request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync();

        foreach (var product in products)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(product.Category);
            var recommendation = new Recommendation
            {
                Title     = product.Title,
                Category  = product.Category,
                Embedding = embedding
            };

            await _recommendationRepository.AddAsync(recommendation);
        }

        await _recommendationRepository.SaveChangesAsync();
    }
}