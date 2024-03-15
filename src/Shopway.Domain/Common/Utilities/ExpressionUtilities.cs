using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using static Shopway.Domain.Common.BaseTypes.ValueObject;

namespace Shopway.Domain.Common.Utilities;

public static class ExpressionUtilities
{
    public static bool IsGeneric(this MemberExpression memberExpression, out Type propertyType)
    {
        var memberPropertyType = ((PropertyInfo)memberExpression.Member).PropertyType;
        var result = memberPropertyType.IsGeneric(out Type genericPropertyTypeOrPropertyType);
        propertyType = genericPropertyTypeOrPropertyType;
        return result;
    }

    /// <summary>
    /// Return a property for a parameter. Support for nested navigation like "Payment.Status"
    /// </summary>
    public static MemberExpression ToMemberExpression(this ParameterExpression parameter, string propertyNameOrPropertyNavigation, bool stopOnCollectionEncounter = false)
    {
        var propertyNavigations = propertyNameOrPropertyNavigation.Split('.');

        var memberExpression = Expression.Property(parameter, propertyNavigations[0]);

        foreach (var propertyNavigator in propertyNavigations.Skip(1))
        {
            if (stopOnCollectionEncounter)
            {
                if (memberExpression.Type.IsEnumerableType())
                {
                    break;
                }
            }

            memberExpression = Expression.Property(memberExpression, propertyNavigator);
        }

        return memberExpression;
    }

    public static ConvertedMember ToConvertedMember(this MemberExpression member)
    {
        if (member.Type.IsValueObject())
        {
            var innerTypeForValueObjectOrCurrentTypeForPrimitive = member.GetValueObjectInnerType();
            var convertedPropertyToFilterOn = member.ConvertToObjectAndThenToGivenType(innerTypeForValueObjectOrCurrentTypeForPrimitive);
            return new ConvertedMember(innerTypeForValueObjectOrCurrentTypeForPrimitive, convertedPropertyToFilterOn);
        }

        return new ConvertedMember(member.Type, member);
    }

    public static Type GetValueObjectInnerType(this MemberExpression expression)
    {
        return expression.Type.GetProperty(Value)!.PropertyType;
    }

    public static UnaryExpression ToConvertedExpression(this Type innerType, object value)
    {
        if (innerType.IsEnum is false)
        {
            return Expression.Convert(Expression.Constant(value), innerType);
        }

        if (value.GetType() == typeof(string))
        {
            if (Enum.TryParse(innerType, (string)value, true, out var result) is false)
            {
                throw new InvalidEnumArgumentException($"Cannot parse {value} to {innerType.Name}. Available values: {Enum.GetNames(innerType).Join(", ")}");
            }

            return Expression.Convert(Expression.Constant(result), innerType);
        }

        return Expression.Convert(Expression.Convert(Expression.Constant(value), typeof(int)), innerType);
    }

    /// <summary>
    /// Convert to ((innerType)(object)property), what is required for value converter approach
    /// </summary>
    /// <param name="property"></param>
    /// <param name="providedType"></param>
    /// <returns></returns>
    public static Expression ConvertToObjectAndThenToGivenType(this Expression property, Type providedType)
    {
        return Expression.Convert(Expression.Convert(property, typeof(object)), providedType);
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

        var left = leftExpression.ReplaceParameter(parameter);
        var right = rightExpression.ReplaceParameter(parameter);

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

        var left = leftExpression.ReplaceParameter(parameter);
        var right = rightExpression.ReplaceParameter(parameter);

        return Expression.Lambda<Func<TType, bool>>(Expression.And(left, right), parameter);
    }

    private static Expression ReplaceParameter<TType>(this Expression<TType> expression, ParameterExpression parameter)
    {
        var visitor = new ReplaceExpressionVisitor(expression.Parameters[0], parameter);
        return visitor.Visit(expression.Body);
    }

    public static Expression ReplaceParameter(this Expression expression, ParameterExpression oldParameter, Expression newExpression)
    {
        var visitor = new ReplaceParameterExpressionVisitor(oldParameter, newExpression);
        return visitor.Visit(expression);
    }

    public static string GetPropertyName<TEntity>(this Expression<Func<TEntity, string>> expression)
    {
        var visitor = new ValueObjectVisitor();
        return visitor.GetPropertyName(expression);
    }

    private sealed class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
    {
        private readonly Expression _oldValue = oldValue;
        private readonly Expression _newValue = newValue;

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

    private sealed class ReplaceParameterExpressionVisitor(ParameterExpression oldParameter, Expression newExpression) : ExpressionVisitor
    {
        private readonly Expression _newExpression = newExpression;
        private readonly ParameterExpression _oldParameter = oldParameter;

        internal static Expression Replace(Expression expression, ParameterExpression oldParameter, Expression newExpression)
        {
            return new ReplaceParameterExpressionVisitor(oldParameter, newExpression)
                .Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression parameter)
        {
            return parameter == _oldParameter 
                ? _newExpression 
                : parameter;
        }
    }

    public class ValueObjectVisitor : ExpressionVisitor
    {
        private string _propertyName = string.Empty;

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.Name is not Value)
            {
                _propertyName = node.Member.Name;
            }

            return base.VisitMember(node);
        }

        public string GetPropertyName<TEntity>(Expression<Func<TEntity, string>> propertyExpression)
        {
            Visit(propertyExpression);
            return _propertyName;
        }
    }

    public sealed record ConvertedMember(Type InnerTypeForValueObjectOrCurrentTypeForPrimitive, Expression ConvertedPropertyToFilterOn);
}