using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;

namespace Shopway.Domain.Enums
{
    public enum Permission
    {
        INVALID_PERMISSION = 1,

        Review_Add = 2,
        Review_Update = 3,
        Review_Remove = 4,
        Review_Read = 5,

        Product_Read = 6,
        Product_Read_Customer = 7,
    }
}

namespace Shopway.Domain.Users.Enumerations
{
    public sealed partial class Permission : Enumeration<Permission>
    {
        private const char _floor = '_';
        public static readonly Permission INVALID_PERMISSION = new(1, nameof(INVALID_PERMISSION));

        public Type? _relatedEntity;

        public Enums.Permission RelatedEnum { get; }
        public PermissionType? Type { get; init; }
        public Type? RelatedAggregateRoot { get; init; }
        public Type? RelatedEntity { get => _relatedEntity is null ? RelatedAggregateRoot : _relatedEntity; init => _relatedEntity = value; }
        public bool HasAllProperties => Properties is null;
        public FrozenSet<string>? Properties { get; init; } = null;

        public Permission(int id, string name)
            : base(id, name)
        {
            if (name.NotContains(_floor))
            {
                throw new ArgumentException($"Permission must contain '{_floor}'.");
            }

            RelatedEnum = Enum.Parse<Enums.Permission>(name);
        }

        // Empty constructor in this case is required by EF Core
        private Permission()
        {
        }
    }

    public enum PermissionType
    {
        Add = 0,
        Update = 1,
        Remove = 2,
        Delete = 3,
        Read = 4,
        Other = 5
    }
}
