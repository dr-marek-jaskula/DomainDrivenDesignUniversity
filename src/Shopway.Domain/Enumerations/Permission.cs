using Shopway.Domain.Abstractions.BaseTypes;

namespace Shopway.Domain.Enumerations
{
    public sealed class Permission : Enumeration<Permission>
    {
        public static readonly Permission Read = new(1, nameof(Read));
        public static readonly Permission Create = new(2, nameof(Create));
        public static readonly Permission Update = new(3, nameof(Update));
        public static readonly Permission Delete = new(4, nameof(Delete));
        public static readonly Permission CRUD_Review = new(5, nameof(CRUD_Review));

        public Permission(int id, string name)
        : base(id, name)
        {
        }

        // Empty constructor in this case is required by EF Core
        private Permission()
        {
        }
    }
}

namespace Shopway.Domain.Enums
{
    public enum Permission
    {
        Read = 1,
        Create = 2,
        Update = 3,
        Delete = 4,
        CRUD_Review = 5
    }
}