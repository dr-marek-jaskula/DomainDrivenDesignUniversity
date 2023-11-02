using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.UtilityTests;

[Trait(nameof(UnitTest), UnitTest.Utility)]
public sealed class ReflectionUtilityTests
{
    [Fact]
    public void Implements_ShouldReturnTrue_WhenTypeImplementsIEntityId()
    {
        //Assert
        var customerIdType = typeof(CustomerId);

        //Act
        var result = customerIdType.Implements<IEntityId>();

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Implements_ShouldReturnFalse_WhenTypeDoesNotImplementsIEntity()
    {
        //Assert
        var customerIdType = typeof(CustomerId);

        //Act
        var result = customerIdType.Implements<IEntity>();

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Implements_ShouldReturnFalse_WhenTypeDoesNotImplementsIEntityId()
    {
        //Assert
        var customerType = typeof(Customer);

        //Act
        var result = customerType.Implements<IEntityId>();

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Implements_ShouldReturnTrue_WhenTypeImplementsIEntity()
    {
        //Assert
        var customerType = typeof(Customer);

        //Act
        var result = customerType.Implements<IEntity>();

        //Assert
        result.Should().BeTrue();
    }
}