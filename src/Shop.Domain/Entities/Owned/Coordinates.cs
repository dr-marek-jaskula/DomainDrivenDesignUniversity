namespace Shopway.Domain.Entities.Owned;

//The Owned reference will be done by db configuration
public sealed class Coordinate
{
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
}