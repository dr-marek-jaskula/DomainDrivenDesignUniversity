using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class RoleName : ValueObject
{
    public readonly static string[] AllowedRoles = new string[4] { "Customer", "Employee", "Manager", "Administrator" };
    public string Value { get; }

    private RoleName(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<RoleName> Create(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return Result.Failure<RoleName>(DomainErrors.RoleName.Empty);
        }

        if (!AllowedRoles.Contains(roleName))
        {
            return Result.Failure<RoleName>(DomainErrors.RoleName.Invalid);
        }

        return new RoleName(roleName);
    }
}