namespace Semantic.Kernel.Domain.Recommendations;

public interface IRecommendationRepository
{
    Task<IEnumerable<Recommendation>> GetSimilarAsync(float[] embedding, int count = 3);
    Task AddAsync(Recommendation recommendation);
    Task SaveChangesAsync();
}