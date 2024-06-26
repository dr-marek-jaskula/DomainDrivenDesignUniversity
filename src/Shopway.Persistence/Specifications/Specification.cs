﻿using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications;

internal sealed class SpecificationWithMapping<TEntity, TEntityId, TOutput> : Specification<TEntity, TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntity : Entity<TEntityId>
{
    internal Expression<Func<TEntity, TOutput>>? MappingExpression { get; private set; } = null;
    internal IMapping<TEntity, TOutput>? Mapping { get; private set; } = null;
    internal bool UseDistinct { get; private set; }

    internal new static SpecificationWithMapping<TEntity, TEntityId, TOutput> New()
    {
        return new SpecificationWithMapping<TEntity, TEntityId, TOutput>();
    }

    internal SpecificationWithMapping<TEntity, TEntityId, TOutput> AddMapping(Expression<Func<TEntity, TOutput>>? mapping)
    {
        MappingExpression = mapping;
        return this;
    }

    internal SpecificationWithMapping<TEntity, TEntityId, TOutput> AddMapping(IMapping<TEntity, TOutput>? mapping)
    {
        Mapping = mapping;
        return this;
    }

    internal SpecificationWithMapping<TEntity, TEntityId, TOutput> ApplyDistinct()
    {
        UseDistinct = true;
        return this;
    }
}

internal class Specification<TEntity, TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntity : Entity<TEntityId>
{
    //Tag
    internal string? QueryTag { get; private set; }

    //Flags
    internal bool AsSplitQuery { get; private set; }
    internal bool AsNoTracking { get; private set; }
    internal bool AsTracking { get; private set; }
    internal bool AsNoTrackingWithIdentityResolution { get; private set; }
    internal bool UseGlobalFilters { get; private set; } = true;

    //Filters (with support for Like operation)
    internal IFilter<TEntity>? Filter { get; private set; } = null;
    internal List<Expression<Func<TEntity, bool>>> FilterExpressions { get; } = [];

    //Like
    internal List<LikeEntry<TEntity>> LikeEntries { get; } = [];
    internal static readonly ILikeProvider<TEntity> LikeProvider = new LikeProvider<TEntity>();

    //SortBy
    internal ISortBy<TEntity>? SortBy { get; private set; } = null;
    internal (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? SortByExpression { get; private set; }
    internal (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? ThenByExpression { get; private set; }

    //Includes
    internal List<string> IncludeStrings { get; } = [];
    internal List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
    internal Func<IQueryable<TEntity>, IQueryable<TEntity>>? IncludeAction { get; private set; } = null;
    internal List<IncludeEntry<TEntity>> IncludeEntries { get; private set; } = [];

    internal static Specification<TEntity, TEntityId> New()
    {
        return new Specification<TEntity, TEntityId>();
    }

    internal Specification<TEntity, TEntityId> AddTag(string queryTag)
    {
        QueryTag = queryTag;
        return this;
    }

    internal Specification<TEntity, TEntityId> IgnoreGlobalFilters()
    {
        UseGlobalFilters = false;
        return this;
    }

    internal Specification<TEntity, TEntityId> UseSplitQuery()
    {
        AsSplitQuery = true;
        return this;
    }

    internal Specification<TEntity, TEntityId> UseNoTracking()
    {
        AsNoTracking = true;
        return this;
    }

    internal Specification<TEntity, TEntityId> UseTracking()
    {
        AsTracking = true;
        return this;
    }

    internal Specification<TEntity, TEntityId> UseNoTrackingWithIdentityResolution()
    {
        AsNoTrackingWithIdentityResolution = true;
        return this;
    }

    internal Specification<TEntity, TEntityId> AddFilter(IFilter<TEntity>? filter)
    {
        Filter = filter;
        return this;
    }

    internal Specification<TEntity, TEntityId> AddFilter(Expression<Func<TEntity, bool>>? filterExpression)
    {
        if (filterExpression is not null)
        {
            FilterExpressions.Add(filterExpression);
        }
        return this;
    }

    internal Specification<TEntity, TEntityId> AddFilters(params Expression<Func<TEntity, bool>>[] filterExpressions)
    {
        foreach (var filterExpression in filterExpressions)
        {
            FilterExpressions.Add(filterExpression);
        }

        return this;
    }

    internal Specification<TEntity, TEntityId> AddLikes(params LikeEntry<TEntity>[] likeEntries)
    {
        foreach (var likeEntry in likeEntries)
        {
            LikeEntries.Add(likeEntry);
        }

        return this;
    }

    internal Specification<TEntity, TEntityId> AddLikes(IList<LikeEntry<TEntity>>? likeEntries)
    {
        if (likeEntries is null)
        {
            return this;
        }

        foreach (var likeEntry in likeEntries)
        {
            LikeEntries.Add(likeEntry);
        }

        return this;
    }

    internal Specification<TEntity, TEntityId> AddLike(Expression<Func<TEntity, string>> property, string likeTerm)
    {
        LikeEntries.Add(new LikeEntry<TEntity>(property, likeTerm));
        return this;
    }

    internal Specification<TEntity, TEntityId> AddSortBy(ISortBy<TEntity>? sortBy)
    {
        SortBy = sortBy;
        return this;
    }

    internal Specification<TEntity, TEntityId> OrderBy(Expression<Func<TEntity, object>> sortByExpression, SortDirection sortDirection)
    {
        SortByExpression = (sortByExpression, sortDirection);
        return this;
    }

    internal Specification<TEntity, TEntityId> ThenBy(Expression<Func<TEntity, object>> thenByExpression, SortDirection sortDirection)
    {
        if (SortByExpression is null)
        {
            throw new InvalidOperationException($"{nameof(SortByExpression)} should be specified before {nameof(ThenByExpression)}");
        }

        if (ThenByExpression is not null)
        {
            throw new InvalidOperationException($"{nameof(ThenByExpression)} can be specified once");
        }

        ThenByExpression = (thenByExpression, sortDirection);
        return this;
    }

    internal Specification<TEntity, TEntityId> AddIncludes(params Expression<Func<TEntity, object>>[] includeExpressions)
    {
        foreach (var includeExpression in includeExpressions)
        {
            IncludeExpressions.Add(includeExpression);
        }

        return this;
    }

    internal Specification<TEntity, TEntityId> AddIncludes(params string[] includeStrings)
    {
        foreach (var includeString in includeStrings)
        {
            IncludeStrings.Add(includeString);
        }

        return this;
    }

    [Obsolete("This is not a preferred way to add includes. Use AddIncludes with action on IIncludeBuilder overload")]
    internal Specification<TEntity, TEntityId> AddIncludeByQueryable(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> includeAction)
    {
        var includeActionBody = includeAction.ToString();

        if (includeActionBody.NotContains("ThenInclude"))
        {
            throw new InvalidOperationException($"Input must contain 'ThenInclude' call. Use builder approach to avoid such issues.");
        }

        if (ContainsMethodCallDifferentFromIncludeOrThenInclude(includeActionBody))
        {
            throw new InvalidOperationException($"Input can only contain 'Include' or 'ThenInclude' calls.");
        }

        IncludeAction = includeAction.Compile();
        return this;
    }

    internal Specification<TEntity, TEntityId> AddIncludes(Action<IIncludeBuilder<TEntity>>? buildIncludes)
    {
        if (buildIncludes is null)
        {
            return this;
        }

        var includeEntries = IncludeBuilderOrchestrator<TEntity>.GetIncludeEntries(buildIncludes);
        IncludeEntries.AddRange(includeEntries);
        return this;
    }

    internal Specification<TEntity, TEntityId> AddIncludes(params IncludeEntry<TEntity>[] includeEntries)
    {
        IncludeEntries.AddRange(includeEntries);
        return this;
    }

    internal Specification<TEntity, TEntityId> AddIncludes(FrozenSet<IncludeEntry<TEntity>> includeEntries)
    {
        IncludeEntries.AddRange(includeEntries);
        return this;
    }

    /// <summary>
    /// Cast specification to mapping specification
    /// </summary>
    /// <typeparam name="TOutput">Output type</typeparam>
    /// <param name="specification">Input specification</param>
    /// <returns>SpecificationWithMappingBase</returns>
    internal SpecificationWithMapping<TEntity, TEntityId, TOutput> AsMappingSpecification<TOutput>()
    {
        return (SpecificationWithMapping<TEntity, TEntityId, TOutput>)this;
    }

    private static bool ContainsMethodCallDifferentFromIncludeOrThenInclude(string includeActionBody)
    {
        return includeActionBody
            .RemoveAll("ThenInclude(", "Include(")
            .Contains('(');
    }
}
