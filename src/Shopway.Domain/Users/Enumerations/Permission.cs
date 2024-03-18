using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Users.Enumerations
{
    public sealed class Permission : Enumeration<Permission>
    {
        private const char floor = '_';

        public static readonly Permission Review_Read = new(1, nameof(Review_Read));
        public static readonly Permission Review_Add = new(2, nameof(Review_Add));
        public static readonly Permission Review_Update = new(3, nameof(Review_Update));
        public static readonly Permission Review_Remove = new(4, nameof(Review_Remove));
        public static readonly Permission INVALID_PERMISSION = new(5, nameof(INVALID_PERMISSION));

        public Permission(int id, string name)
            : base(id, name)
        {
            if (name.NotContains(floor))
            {
                throw new ArgumentException($"Permission must contain {floor}.");
            }
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
        Review_Read = 1,
        Review_Add = 2,
        Review_Update = 3,
        Review_Remove = 4,
        INVALID_PERMISSION = 5,
    }
}