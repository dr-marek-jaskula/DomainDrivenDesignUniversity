using NetArchTest.Rules;
using Shopway.Domain.BaseTypes;

namespace Shopway.Tests.Unit.SystemsUnderTest.Architecture;

public sealed class ValueObjects
{
    [Fact]
    public void ValueObjects_Should_Be_Immutable()
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
}