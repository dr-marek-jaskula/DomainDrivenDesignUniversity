using static System.Reflection.BindingFlags;

namespace Shopway.Domain.Users.Authorization;

public enum RoleName
{
    Customer = 1,
    Employee = 2,
    Manager = 3,
    Administrator = 4
}

public sealed class Role
{
    public static readonly Role Customer = new(nameof(Customer));
    public static readonly Role Employee = new(nameof(Employee));
    public static readonly Role Manager = new(nameof(Manager));
    public static readonly Role Administrator = new(nameof(Administrator));

    public Role(string name)
    {
        Name = name;
    }

    //Empty constructor in this case is required by EF Core
    private Role()
    {
    }

    public string Name { get; init; }
    public ICollection<User> Users { get; init; }
    public ICollection<Permission> Permissions { get; init; }

    public static List<Role> GetPredefinedRoles()
    {
        return typeof(Role)
            .GetFields(Public | Static | Instance)
            .Where(x => x.DeclaringType == typeof(Role))
            .Select(x => x.GetValue(null))
            .Cast<Role>()
            .ToList();
    }

    public RoleName GetRelatedEnum()
    {
        return Enum.Parse<RoleName>(Name);
    }
}
