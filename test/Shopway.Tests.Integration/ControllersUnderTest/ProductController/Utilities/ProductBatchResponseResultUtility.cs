using Shopway.Application.Features;
using Shopway.Domain.Errors;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;

public static class ProductBatchResponseResultUtility
{
    public static void ShouldHaveCount(this ProductBatchResponseResult productBatchResponseResult, int count, BatchEntryStatus batchEntryStatus)
    {
        productBatchResponseResult!
            .Entries
            .Where(x => x.Status == batchEntryStatus)
            .Should()
            .HaveCount(count);
    }

    public static void ShouldHaveCount(this ProductBatchResponseResult productBatchResponseResult, int count)
    {
        productBatchResponseResult!
            .Entries
            .Should()
            .HaveCount(count);
    }

    public static void ShouldContainEntryWithErrors(this ProductBatchResponseResult productBatchResponseResult, params Error[] errors)
    {
        var errorEntry = productBatchResponseResult!
            .Entries
            .Where(x => x.Status is BatchEntryStatus.Error)
            .First();

        errorEntry
            .Errors
            .Should()
            .Contain(errors);
    }
}