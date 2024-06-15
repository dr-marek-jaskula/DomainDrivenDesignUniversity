using Shopway.Domain.Common.BaseTypes;

namespace Shopway.Domain.Users.Enumerations
{
    public sealed class Role : Enumeration<Role>
    {
        public static readonly Role Customer = new(1, nameof(Customer));
        public static readonly Role Employee = new(2, nameof(Employee));
        public static readonly Role Manager = new(3, nameof(Manager));
        public static readonly Role Administrator = new(4, nameof(Administrator));

        public Role(int id, string name)
            : base(id, name)
        {
        }

        //Empty constructor in this case is required by EF Core
        private Role()
        {
        }

        public ICollection<User> Users { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}

namespace Shopway.Domain.Enums
{
    public enum Role
    {
        Customer = 1,
        Employee = 2,
        Manager = 3,
        Administrator = 4
    }
}
