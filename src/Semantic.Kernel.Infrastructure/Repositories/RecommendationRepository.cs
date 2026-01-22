using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using Semantic.Kernel.Infrastructure.Data;
using Semantic.Kernel.Domain.Recommendations;

namespace Semantic.Kernel.Infrastructure.Repositories;

public class RecommendationRepository : IRecommendationRepository
{
    private readonly AppDbContext _context;

    public RecommendationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Recommendation>> GetSimilarAsync(float[] embedding, int count = 3)
    {
        var vector = new Pgvector.Vector(embedding);
        
        return await _context.Recommendations
            .AsNoTracking()
            .OrderBy(r => EF.Property<Pgvector.Vector>(r, "Embedding").CosineDistance(vector))
            .Take(count)
            .Select(r => new Recommendation
            {
                Id        = r.Id,
                Title     = r.Title,
                Category  = r.Category,
                Embedding = r.Embedding
            })
            .ToListAsync();
    }

    public async Task AddAsync(Recommendation recommendation)
    {
        await _context.Recommendations.AddAsync(recommendation);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}