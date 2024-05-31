using FastEndpoints;
using NetArchTest.Rules;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public partial class NamingConventionsTests
{
    [Fact]
    public void EndpointsNames_ShouldEndWithEndpoint()
    {
        //Arrange
        var assembly = Shopway.Presentation.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .AreClasses()
            .And()
            .Inherit(typeof(BaseEndpoint))
            .Should()
            .HaveNameEndingWith(Endpoint)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
