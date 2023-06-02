using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Abstractions;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters;

public sealed class EntityIdComparer : ValueComparer<IEntityId>
{
    public EntityIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) {}
}

public sealed class CustomerIdConverter : ValueConverter<CustomerId, Guid>
{
    public CustomerIdConverter() : base(id => id.Value, guid => CustomerId.Create(guid)) { }
}

public sealed class OrderHeaderIdConverter : ValueConverter<OrderHeaderId, Guid>
{
    public OrderHeaderIdConverter() : base(id => id.Value, guid => OrderHeaderId.Create(guid)) { }
}

public sealed class OrderLineIdConverter : ValueConverter<OrderLineId, Guid>
{
    public OrderLineIdConverter() : base(id => id.Value, guid => OrderLineId.Create(guid)) { }
}

public sealed class PaymentIdConverter : ValueConverter<PaymentId, Guid>
{
    public PaymentIdConverter() : base(id => id.Value, guid => PaymentId.Create(guid)) { }
}
public sealed class ProductIdConverter : ValueConverter<ProductId, Guid>
{
    public ProductIdConverter() : base(id => id.Value, guid => ProductId.Create(guid)) { }
}

public sealed class ReviewIdConverter : ValueConverter<ReviewId, Guid>
{
    public ReviewIdConverter() : base(id => id.Value, guid => ReviewId.Create(guid)) { }
}

public sealed class UserIdConverter : ValueConverter<UserId, Guid>
{
    public UserIdConverter() : base(id => id.Value, guid => UserId.Create(guid)) { }
}
