using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Domain.Common.DataProcessing;

public class IncludeBuilderOrchestrator<TEntity> : IIncludeBuilder<TEntity>
    where TEntity : class, IEntity
{
    private readonly List<IncludeBuilder<TEntity>> _includeBuilders = [];

    public static IEnumerable<IncludeEntry<TEntity>> GetIncludeEntries(Action<IIncludeBuilder<TEntity>> buildIncludes)
    {
        var orchestrator = new IncludeBuilderOrchestrator<TEntity>();
        buildIncludes(orchestrator);
        return orchestrator.GetIncludeEntries();
    }

    public IThenIncludeBuilder<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
        where TProperty : class, IEntity
    {
        var newIncludeBuilder = new IncludeBuilder<TEntity>(this);
        _includeBuilders.Add(newIncludeBuilder);
        return newIncludeBuilder.Include(includeExpression);
    }

    public IThenIncludeBuilder<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includeExpression)
        where TProperty : class, IEntity
    {
        var newIncludeBuilder = new IncludeBuilder<TEntity>(this);
        _includeBuilders.Add(newIncludeBuilder);
        return newIncludeBuilder.Include(includeExpression);
    }

    private IEnumerable<IncludeEntry<TEntity>> GetIncludeEntries()
    {
        return _includeBuilders
            .SelectMany(x => x.IncludeEntries);
    }
}

internal class IncludeBuilder<TEntity>(IncludeBuilderOrchestrator<TEntity> includeBuilderOrchestrator) : IIncludeBuilder<TEntity>
    where TEntity : class, IEntity
{
    internal readonly List<IncludeEntry<TEntity>> IncludeEntries = [];
    private readonly IncludeBuilderOrchestrator<TEntity> _includeBuilderOrchestrator = includeBuilderOrchestrator;

    public IThenIncludeBuilder<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
        where TProperty : class, IEntity
    {
        var includeEntry = new IncludeEntry<TEntity>(includeExpression, typeof(TEntity), typeof(TProperty), null, IncludeType.Include);
        IncludeEntries.Add(includeEntry);
        return new ThenIncludeBuilder<TEntity, TProperty>(_includeBuilderOrchestrator, this, false);
    }

    public IThenIncludeBuilder<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includeExpression)
        where TProperty : class, IEntity
    {
        var includeEntry = new IncludeEntry<TEntity>(includeExpression, typeof(TEntity), typeof(IEnumerable<TProperty>), null, IncludeType.Include);
        IncludeEntries.Add(includeEntry);
        return new ThenIncludeBuilder<TEntity, TProperty>(_includeBuilderOrchestrator, this, true);
    }
}

internal class ThenIncludeBuilder<TEntity, TProperty>
(
    IncludeBuilderOrchestrator<TEntity> includeBuilderOrchestrator, 
    IncludeBuilder<TEntity> includeBuilder, 
    bool previousWasCollection
) 
    : IThenIncludeBuilder<TEntity, TProperty>
        where TEntity : class, IEntity
        where TProperty : class, IEntity
{
    private readonly IncludeBuilderOrchestrator<TEntity> _includeBuilderOrchestrator = includeBuilderOrchestrator;
    private readonly IncludeBuilder<TEntity> _includeBuilder = includeBuilder;
    private readonly bool _previousWasCollection = previousWasCollection;

    public IThenIncludeBuilder<TEntity, TNewProperty> Include<TNewProperty>(Expression<Func<TEntity, TNewProperty>> includeExpression)
        where TNewProperty : class, IEntity
    {
        return _includeBuilderOrchestrator.Include(includeExpression);
    }

    public IThenIncludeBuilder<TEntity, TNewProperty> Include<TNewProperty>(Expression<Func<TEntity, IEnumerable<TNewProperty>>> includeExpression)
        where TNewProperty : class, IEntity
    {
        return _includeBuilderOrchestrator.Include(includeExpression);
    }

    public IThenIncludeBuilder<TEntity, TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, TNextProperty>> thenIncludeExpression)
        where TNextProperty : class, IEntity
    {
        Type fullPreviousType = _previousWasCollection 
            ? typeof(IEnumerable<TProperty>)
            : typeof(TProperty);

        var includeEntry = new IncludeEntry<TEntity>(thenIncludeExpression, typeof(TEntity), typeof(TNextProperty), fullPreviousType, IncludeType.ThenInclude);

        _includeBuilder.IncludeEntries.Add(includeEntry);
        return new ThenIncludeBuilder<TEntity, TNextProperty >(_includeBuilderOrchestrator, _includeBuilder, false);
    }

    public IThenIncludeBuilder<TEntity, TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, IEnumerable<TNextProperty>>> thenIncludeExpression)
        where TNextProperty : class, IEntity
    {
        Type fullPreviousType = _previousWasCollection 
            ? typeof(IEnumerable<TProperty>)
            : typeof(TProperty);

        var includeEntry = new IncludeEntry<TEntity>(thenIncludeExpression, typeof(TEntity), typeof(IEnumerable<TNextProperty>), fullPreviousType, IncludeType.ThenInclude);

        _includeBuilder.IncludeEntries.Add(includeEntry);
        return new ThenIncludeBuilder<TEntity, TNextProperty>(_includeBuilderOrchestrator, _includeBuilder, true);
    }
}
