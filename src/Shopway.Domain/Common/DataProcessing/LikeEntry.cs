using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Domain.Common.DataProcessing;

public sealed record LikeEntry<TEntity>(Expression<Func<TEntity, string>> Property, string LikeTerm)
    where TEntity : class, IEntity;
