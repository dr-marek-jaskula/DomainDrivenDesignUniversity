using NetArchTest.Rules;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Tests.Unit.ArchitectureTests.Utilities;

namespace Shopway.Tests.Unit.ArchitectureTests;

[UnitTest.Architecture]
public sealed class ValueObjectTests
{
    [Fact]
    public void IValueObjects_ShouldBeImmutable()
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IValueObject))
            .And()
            .AreNotAbstract()
            .Should()
            .BeImmutable()
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Theory]
    [InlineData(IValueObject.ValidateMethodName)]
    [InlineData(IValueObject.CreateMethodName)]
    public void IValueObjects_ShouldDefineMethod(string methodName)
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IValueObject))
            .And()
            .AreNotAbstract()
            .Should()
            .DefineMethod(methodName)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
