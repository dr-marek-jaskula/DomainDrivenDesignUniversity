using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;

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
        if (!AllowedRoles.Contains(roleName))
        {
            return Result.Failure<RoleName>(RoleNameError.Invalid);
        }

        return new RoleName(roleName);
    }
}