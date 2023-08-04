using NetArchTest.Rules;
using Shopway.Domain.BaseTypes;
using Shopway.Tests.Unit.ArchitectureTests.CustomRules;
using Shopway.Tests.Unit.Constants;

namespace Shopway.Tests.Unit.ArchitectureTests;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public sealed class ValueObjectTests
{
    [Fact]
    public void ValueObjects_ShouldBeImmutable()
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(ValueObject))
            .Should()
            .BeImmutable()
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Theory]
    [InlineData("Validate")]
    [InlineData("Create")]
    public void ValueObjects_ShouldDefineMethod(string methodName)
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(ValueObject))
            .Should()
            .MeetCustomRule(new ContainsMethod(methodName))
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}