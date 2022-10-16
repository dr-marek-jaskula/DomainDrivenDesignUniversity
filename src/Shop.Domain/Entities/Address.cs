using Shopway.Domain.Entities.Owned;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public class Address : Entity
{
    public string? City { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;
    public string? ZipCode { get; set; }
    public string? Street { get; set; }
    public int? Building { get; set; }
    public int? Flat { get; set; }
    public virtual Person? Person { get; set; }
    public virtual Shop? Shop { get; set; }

    //Owned reference
    public Coordinate? Coordinate { get; set; }
}