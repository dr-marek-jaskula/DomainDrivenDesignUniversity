using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using Shopway.Domain.Users;
using static Shopway.Domain.Common.BaseTypes.IEntityUtilities;

namespace Shopway.Tests.Unit.UtilityTests;

[UnitTest.Utility]
public sealed class IEntityUtilityTests
{
    private static readonly List<string> _productAllPropertiesWithNestedProperties = ["Id", "Price", "Revision", "ProductName", "UomCode", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy", "Reviews.ProductId", "Reviews.Id", "Reviews.Description", "Reviews.Title", "Reviews.Username", "Reviews.Stars", "Reviews.CreatedOn", "Reviews.CreatedBy", "Reviews.UpdatedOn", "Reviews.UpdatedBy"];
    private static readonly List<string> _orderLineAllPropertiesWithNestedProperties = ["Amount", "LineDiscount", "CreatedOn", "UpdatedOn", "CreatedBy", "UpdatedBy", "OrderHeaderId", "Id", "ProductSummary.ProductId", "ProductSummary.ProductName", "ProductSummary.Revision", "ProductSummary.Price", "ProductSummary.UomCode"];
    private static readonly List<string> _orderHeaderAllPropertiesWithNestedProperties = ["TotalDiscount", "Status", "CreatedOn", "UpdatedOn", "CreatedBy", "UpdatedBy", "UserId", "SoftDeletedOn", "SoftDeleted", "Id", "OrderLines.Amount", "OrderLines.LineDiscount", "OrderLines.CreatedOn", "OrderLines.UpdatedOn", "OrderLines.CreatedBy", "OrderLines.UpdatedBy", "OrderLines.OrderHeaderId", "OrderLines.Id", "OrderLines.ProductSummary.ProductId", "OrderLines.ProductSummary.ProductName", "OrderLines.ProductSummary.Revision", "OrderLines.ProductSummary.Price", "OrderLines.ProductSummary.UomCode", "Payments.IsRefunded", "Payments.OrderHeaderId", "Payments.Status", "Payments.CreatedOn", "Payments.UpdatedOn", "Payments.CreatedBy", "Payments.UpdatedBy", "Payments.Id", "Payments.Session.Id", "Payments.Session.Secret", "Payments.Session.PaymentIntentId"];
    private static readonly List<string> _userAllPropertiesWithNestedProperties = ["Username", "Email", "CreatedOn", "UpdatedOn", "CreatedBy", "UpdatedBy", "PasswordHash", "CustomerId", "ExternalIdentityProvider", "RefreshToken", "TwoFactorTokenHash", "TwoFactorToptSecret", "TwoFactorTokenCreatedOn", "Roles", "Id", "Customer.FirstName", "Customer.LastName", "Customer.Gender", "Customer.Rank", "Customer.DateOfBirth", "Customer.PhoneNumber", "Customer.CreatedOn", "Customer.UpdatedOn", "Customer.CreatedBy", "Customer.UpdatedBy", "Customer.UserId", "Customer.Id", "Customer.Address.City", "Customer.Address.Country", "Customer.Address.ZipCode", "Customer.Address.Street", "Customer.Address.Building", "Customer.Address.Flat", "Customer.User.Username", "Customer.User.Email", "Customer.User.CreatedOn", "Customer.User.UpdatedOn", "Customer.User.CreatedBy", "Customer.User.UpdatedBy", "Customer.User.PasswordHash", "Customer.User.CustomerId", "Customer.User.ExternalIdentityProvider", "Customer.User.RefreshToken", "Customer.User.TwoFactorTokenHash", "Customer.User.TwoFactorTokenCreatedOn", "Customer.User.TwoFactorToptSecret", "Customer.User.Roles", "Customer.User.Id"];

    [Fact]
    public void GetAggregatePropertiesWithHierarchy_ShouldReturnAllProductProperties_WhenProductIsGivenAsInput()
    {
        //Arrange
        var entityName = nameof(Product);

        //Act
        var result = GetAggregatePropertiesWithHierarchy(entityName);

        //Assert
        result.Should().BeEquivalentTo(_productAllPropertiesWithNestedProperties);
    }

    [Fact]
    public void GetEntityPropertiesWithHierarchy_ShouldReturnAllOrderHeaderProperties_WhenOrderHeaderIsGivenAsInput()
    {
        //Arrange
        var entityName = nameof(OrderHeader);

        //Act
        var result = GetAggregatePropertiesWithHierarchy(entityName);

        //Assert
        result.Should().BeEquivalentTo(_orderHeaderAllPropertiesWithNestedProperties);
    }

    [Fact]
    public void GetEntityPropertiesWithHierarchy_ShouldReturnAllUserPropertiesWithoutCircularDependencyIssue_WhenUserIsGivenAsInput()
    {
        //Arrange
        var entityName = nameof(User);

        //Act
        var result = GetAggregatePropertiesWithHierarchy(entityName);

        //Assert
        result.Should().BeEquivalentTo(_userAllPropertiesWithNestedProperties);
    }

    [Fact]
    public void GetEntityPropertiesWithHierarchy_ShouldNotReturnAllOrderLineProperties_WhenOrderLineIsGivenAsInput()
    {
        //Arrange
        var entityName = nameof(OrderLine);

        //Act
        var action = () => GetAggregatePropertiesWithHierarchy(entityName);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }
}
