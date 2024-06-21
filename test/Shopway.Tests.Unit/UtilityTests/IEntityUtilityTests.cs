using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using static Shopway.Domain.Common.BaseTypes.IEntityUtilities;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.UtilityTests;

[Trait(nameof(UnitTest), UnitTest.Utility)]
public sealed class IEntityUtilityTests
{
    private static readonly List<string> _productAllPropertiesWithNestedProperties = ["Id", "Price", "Revision", "ProductName", "UomCode", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy", "Reviews", "Reviews.ProductId", "Reviews.Id", "Reviews.Description", "Reviews.Title", "Reviews.Username", "Reviews.Stars", "Reviews.CreatedOn", "Reviews.CreatedBy", "Reviews.UpdatedOn", "Reviews.UpdatedBy"];
    private static readonly List<string> _orderLineAllPropertiesWithNestedProperties = ["Amount", "LineDiscount", "CreatedOn", "UpdatedOn", "CreatedBy", "UpdatedBy", "ProductSummary", "OrderHeaderId", "Id", "ProductSummary.ProductId", "ProductSummary.ProductName", "ProductSummary.Revision", "ProductSummary.Price", "ProductSummary.UomCode"];
    private static readonly List<string> _ordeHeaderAllPropertiesWithNestedProperties = ["TotalDiscount", "Status", "CreatedOn", "UpdatedOn", "CreatedBy", "UpdatedBy", "UserId", "OrderLines", "Payments", "SoftDeletedOn", "SoftDeleted", "Id", "OrderLines.Amount", "OrderLines.LineDiscount", "OrderLines.CreatedOn", "OrderLines.UpdatedOn", "OrderLines.CreatedBy", "OrderLines.UpdatedBy", "OrderLines.ProductSummary", "OrderLines.OrderHeaderId", "OrderLines.Id", "OrderLines.ProductSummary.ProductId", "OrderLines.ProductSummary.ProductName", "OrderLines.ProductSummary.Revision", "OrderLines.ProductSummary.Price", "OrderLines.ProductSummary.UomCode", "Payments.Session", "Payments.IsRefunded", "Payments.OrderHeaderId", "Payments.Status", "Payments.CreatedOn", "Payments.UpdatedOn", "Payments.CreatedBy", "Payments.UpdatedBy", "Payments.Id", "Payments.Session.Id", "Payments.Session.Secret", "Payments.Session.PaymentIntentId"];

    [Fact]
    public void GetEntityPropertiesWithHierarchy_ShouldReturnAllProductProperties_WhenProductIsGivenAsInput()
    {
        //Arrange
        var entityName = nameof(Product);

        //Act
        var result = GetEntityPropertiesWithHierarchy(entityName);

        //Assert
        result.Should().BeEquivalentTo(_productAllPropertiesWithNestedProperties);
    }

    [Fact]
    public void GetEntityPropertiesWithHierarchy_ShouldReturnAllOrderLineProperties_WhenOrderLineIsGivenAsInput()
    {
        //Arrange
        var entityName = nameof(OrderLine);

        //Act
        var result = GetEntityPropertiesWithHierarchy(entityName);

        //Assert
        result.Should().BeEquivalentTo(_orderLineAllPropertiesWithNestedProperties);
    }

    [Fact]
    public void GetEntityPropertiesWithHierarchy_ShouldReturnAllOrderHeaderProperties_WhenOrderHeaderIsGivenAsInput()
    {
        //Arrange
        var entityName = nameof(OrderHeader);

        //Act
        var result = GetEntityPropertiesWithHierarchy(entityName);

        //Assert
        result.Should().BeEquivalentTo(_ordeHeaderAllPropertiesWithNestedProperties);
    }
}
