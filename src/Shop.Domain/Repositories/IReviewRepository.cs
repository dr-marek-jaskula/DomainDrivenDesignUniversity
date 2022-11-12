using Shopway.Domain.Entities;

namespace Shopway.Domain.Repositories;

public interface IReviewRepository
{
    void Add(Review review);

    void Remove(Review review);
}