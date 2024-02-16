using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications;

internal partial class Specification<TEntity, TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntity : Entity<TEntityId>
{
    internal interface IIncludeBuilder
    {
        IThenIncludeBuilder<TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
            where TProperty : class, IEntity;
        IThenIncludeBuilder<TProperty> Include<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includeExpression)
            where TProperty : class, IEntity;
    }

    internal interface IThenIncludeBuilder<TProperty> : IIncludeBuilder
    {
        IThenIncludeBuilder<TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, TNextProperty>> thenIncludeExpression)
            where TNextProperty : class, IEntity;
        IThenIncludeBuilder<TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, IEnumerable<TNextProperty>>> thenIncludeExpression)
            where TNextProperty : class, IEntity;
    }

    internal class IncludeBuilderOrchestrator : IIncludeBuilder
    {
        private readonly List<IncludeBuilder> _includeBuilders = [];

        public IThenIncludeBuilder<TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
            where TProperty : class, IEntity
        {
            var newIncludeBuilder = new IncludeBuilder(this);
            _includeBuilders.Add(newIncludeBuilder);
            return newIncludeBuilder.Include(includeExpression);
        }

        public IThenIncludeBuilder<TProperty> Include<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includeExpression)
            where TProperty : class, IEntity
        {
            var newIncludeBuilder = new IncludeBuilder(this);
            _includeBuilders.Add(newIncludeBuilder);
            return newIncludeBuilder.Include(includeExpression);
        }

        internal List<IncludeEntry<TEntity>> GetIncludeEntries()
        {
            return _includeBuilders
                .SelectMany(x => x.IncludeEntries)
                .ToList();
        }
    }

    internal class IncludeBuilder(IncludeBuilderOrchestrator includeBuilderOrchestrator) : IIncludeBuilder
    {
        internal readonly List<IncludeEntry<TEntity>> IncludeEntries = [];
        private readonly IncludeBuilderOrchestrator _includeBuilderOrchestrator = includeBuilderOrchestrator;

        public IThenIncludeBuilder<TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
            where TProperty : class, IEntity
        {
            var includeEntry = new IncludeEntry<TEntity>(includeExpression, typeof(TEntity), typeof(TProperty), null, IncludeType.Include);
            IncludeEntries.Add(includeEntry);
            return new ThenIncludeBuilder<TProperty>(_includeBuilderOrchestrator, this, false);
        }

        public IThenIncludeBuilder<TProperty> Include<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includeExpression)
            where TProperty : class, IEntity
        {
            var includeEntry = new IncludeEntry<TEntity>(includeExpression, typeof(TEntity), typeof(IEnumerable<TProperty>), null, IncludeType.Include);
            IncludeEntries.Add(includeEntry);
            return new ThenIncludeBuilder<TProperty>(_includeBuilderOrchestrator, this, true);
        }
    }

    internal class ThenIncludeBuilder<TProperty>(IncludeBuilderOrchestrator includeBuilderOrchestrator, IncludeBuilder includeBuilder, bool previousWasCollection) : IThenIncludeBuilder<TProperty>
        where TProperty : class, IEntity
    {
        private readonly IncludeBuilderOrchestrator _includeBuilderOrchestrator = includeBuilderOrchestrator;
        private readonly IncludeBuilder _includeBuilder = includeBuilder;
        private readonly bool _previousWasCollection = previousWasCollection;

        public IThenIncludeBuilder<TNewProperty> Include<TNewProperty>(Expression<Func<TEntity, TNewProperty>> includeExpression)
            where TNewProperty : class, IEntity
        {
            return _includeBuilderOrchestrator.Include(includeExpression);
        }

        public IThenIncludeBuilder<TNewProperty> Include<TNewProperty>(Expression<Func<TEntity, IEnumerable<TNewProperty>>> includeExpression)
            where TNewProperty : class, IEntity
        {
            return _includeBuilderOrchestrator.Include(includeExpression);
        }

        public IThenIncludeBuilder<TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, TNextProperty>> thenIncludeExpression)
            where TNextProperty : class, IEntity
        {
            Type fullPreviousType = _previousWasCollection 
                ? typeof(IEnumerable<TProperty>)
                : typeof(TProperty);

            var includeEntry = new IncludeEntry<TEntity>(thenIncludeExpression, typeof(TEntity), typeof(TNextProperty), fullPreviousType, IncludeType.ThenInclude);

            _includeBuilder.IncludeEntries.Add(includeEntry);
            return new ThenIncludeBuilder<TNextProperty>(_includeBuilderOrchestrator, _includeBuilder, false);
        }

        public IThenIncludeBuilder<TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, IEnumerable<TNextProperty>>> thenIncludeExpression)
            where TNextProperty : class, IEntity
        {
            Type fullPreviousType = _previousWasCollection 
                ? typeof(IEnumerable<TProperty>)
                : typeof(TProperty);

            var includeEntry = new IncludeEntry<TEntity>(thenIncludeExpression, typeof(TEntity), typeof(IEnumerable<TNextProperty>), fullPreviousType, IncludeType.ThenInclude);

            _includeBuilder.IncludeEntries.Add(includeEntry);
            return new ThenIncludeBuilder<TNextProperty>(_includeBuilderOrchestrator, _includeBuilder, true);
        }
    }
}