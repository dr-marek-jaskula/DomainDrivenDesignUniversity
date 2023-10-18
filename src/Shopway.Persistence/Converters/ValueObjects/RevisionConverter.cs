using Shopway.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class RevisionConverter : ValueConverter<Revision, string>
{
    public RevisionConverter() : base(revision => revision.Value, @string => Revision.Create(@string).Value) { }
}

public sealed class RevisionComparer : ValueComparer<Revision>
{
    public RevisionComparer() : base((revision1, revision2) => revision1!.Value == revision2!.Value, revision => revision.GetHashCode()) { }
}