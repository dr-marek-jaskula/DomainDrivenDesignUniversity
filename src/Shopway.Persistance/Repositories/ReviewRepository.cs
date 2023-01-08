using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Repositories;

public sealed class ReviewRepository : BaseRepository, IReviewRepository
{
    public ReviewRepository(ShopwayDbContext dbContext) : base(dbContext)
    {
    }

    public void Add(Review review)
    {
        _dbContext
            .Set<Review>()
            .Add(review);
    }

    public void Remove(Review review)
    {
        _dbContext
            .Set<Review>()
            .Remove(review);
    }

    public void Update(Review review)
    {
        _dbContext
            .Set<Review>()
            .Update(review);
    }
}