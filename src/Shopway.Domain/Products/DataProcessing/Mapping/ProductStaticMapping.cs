using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Products.DataProcessing.Mapping;

public sealed record ProductStaticMapping : IMapping<Product, DataTransferObject>
{
    public static readonly ProductStaticMapping Instance = new()
    {
        Id = true,
        ProductName = true,
        Revision = true,
        Price = true,
        UomCode = true,
    };

    public bool? Id { get; init; }
    public bool? ProductName { get; init; }
    public bool? Revision { get; init; }
    public bool? Price { get; init; }
    public bool? UomCode { get; init; }
    public ReviewStaticMapping Reviews { get; init; } = new();

    private bool IncludeId => Id is true;
    private bool IncludeProductName => ProductName is true;
    private bool IncludeRevision => Revision is true;
    private bool IncludePrice => Price is true;
    private bool IncludeUomCode => UomCode is true;
    private bool IncludeReviews => Reviews.AnySelected();

    public IQueryable<DataTransferObject> Apply(IQueryable<Product> queryable)
    {
        //WARNING! this static query will result in querying ALL fields from the database and then map to ones we require.
        //This is caused by the behavior of EF Core - if the property is present in the select statement it will be queried.
        //To solve this problem we could also create collection of MappingEntries from all given values and then use queryable.Map(mappingEnties)
        //Then, only the required field would be queried. I left this solution for demonstration purposes.
        //If someone decides that performance reasons are nos crucial for him and the following syntax looks good, then You can stick with this
        //My personal option would be to use queryable.Map(mappingEntries) approach, for !performance! and !maintenance! reasons.
        return queryable
            .Select(product => new DataTransferObject()
                .AddIf(IncludeId, nameof(product.Id), $"{product.Id}")
                .AddIf(IncludeUomCode, nameof(product.UomCode), $"{product.UomCode}")
                .AddIf(IncludePrice, nameof(product.Price), $"{product.Price}")
                .AddIf(IncludeRevision, nameof(product.Revision), $"{product.Revision}")
                .AddIf(IncludeProductName, nameof(product.ProductName), $"{product.ProductName}")
                .AddIf(IncludeReviews, nameof(product.Reviews), product.Reviews.Select(review => new DataTransferObject()
                    .AddIf(Reviews!.IncludeUsername, nameof(review.Username), $"{review.Username}")
                    .AddIf(Reviews!.IncludeStars, nameof(review.Stars), $"{review.Stars}")
                    .AddIf(Reviews!.IncludeTitle, nameof(review.Title), $"{review.Title}")
                    .AddIf(Reviews!.IncludeDescription, nameof(review.Description), $"{review.Description}"))));
    }

    public sealed class ReviewStaticMapping
    {
        public bool? Username { get; init; }
        public bool? Stars { get; init; }
        public bool? Title { get; init; }
        public bool? Description { get; init; }

        public bool IncludeUsername => Username is true;
        public bool IncludeStars => Stars is true;
        public bool IncludeTitle => Title is true;
        public bool IncludeDescription => Description is true;

        public bool AnySelected()
        {
            return IncludeUsername
                || IncludeStars
                || IncludeTitle
                || IncludeDescription;
        }
    }
}