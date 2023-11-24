using System.Linq.Expressions;
using static Shopway.Domain.Common.BaseTypes.ValueObject;

namespace Shopway.Domain.Common.Utilities;

public static class ExpressionUtilities
{
    public static MemberExpression ToMemberExpression(this ParameterExpression parameter, string propertyName)
    {
        return Expression.Property(parameter, propertyName);
    }

    public static Type GetValueObjectInnerType(this MemberExpression expression)
    {
        return expression.Type.GetProperty(Value)!.PropertyType;
    }

    public static UnaryExpression ToConvertedExpression(this Type innerType, object value)
    {
        return Expression.Convert(Expression.Constant(value), innerType);
    }

    /// <summary>
    /// Convert to ((innerType)(object)property), what is required for value converter approach
    /// </summary>
    /// <param name="property"></param>
    /// <param name="innerType"></param>
    /// <returns></returns>
    public static Expression ConvertInnerValueToInnerTypeAndObject(this MemberExpression property, Type innerType)
    {
        return Expression.Convert(Expression.Convert(property, typeof(object)), innerType);
    }

    public static Expression<Func<TType, bool>> Or<TType>(params Expression<Func<TType, bool>>[] expressions)
    {
        Expression<Func<TType, bool>> result = expressions.First();

        foreach (var expression in expressions[1..])
        {
            result = result.Or(expression);
        }

        return result;
    }

    public static Expression<Func<TType, bool>> Or<TType>(this Expression<Func<TType, bool>> leftExpression, Expression<Func<TType, bool>> rightExpression)
    {
        var parameter = Expression.Parameter(typeof(TType));

        var left = ReplaceParameter(leftExpression, parameter);
        var right = ReplaceParameter(rightExpression, parameter);

        return Expression.Lambda<Func<TType, bool>>(Expression.Or(left, right), parameter);
    }

    public static Expression<Func<TType, bool>> And<TType>(params Expression<Func<TType, bool>>[] expressions)
    {
        Expression<Func<TType, bool>> result = expressions.First();

        foreach (var expression in expressions[1..])
        {
            result = result.And(expression);
        }

        return result;
    }

    public static Expression<Func<TType, bool>> And<TType>(this Expression<Func<TType, bool>> leftExpression, Expression<Func<TType, bool>> rightExpression)
    {
        var parameter = Expression.Parameter(typeof(TType));

        var left = ReplaceParameter(leftExpression, parameter);
        var right = ReplaceParameter(rightExpression, parameter);

        return Expression.Lambda<Func<TType, bool>>(Expression.And(left, right), parameter);
    }

    private static Expression ReplaceParameter<TType>(Expression<TType> expression, ParameterExpression parameter)
    {
        var visitor = new ReplaceExpressionVisitor(expression.Parameters[0], parameter);
        return visitor.Visit(expression.Body);
    }

    private sealed class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression? node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            return node == _oldValue
                ? _newValue
                : base.Visit(node);
        }
    }
}