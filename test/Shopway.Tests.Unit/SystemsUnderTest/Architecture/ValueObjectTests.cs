using NetArchTest.Rules;
using Shopway.Domain.BaseTypes;
using Shopway.Tests.Unit.SystemsUnderTest.Architecture.CustomRules;

namespace Shopway.Tests.Unit.SystemsUnderTest.Architecture;

public sealed class ValueObjectTests
{
    [Fact]
    public void ValueObjects_ShouldBeImmutable()
    {
        //Arrange
        var assembly = Shopway.Domain.AssemblyReference.Assembly;

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
        var assembly = Shopway.Domain.AssemblyReference.Assembly;

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