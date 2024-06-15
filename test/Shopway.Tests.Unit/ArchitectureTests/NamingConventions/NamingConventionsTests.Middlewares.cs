using Microsoft.AspNetCore.Http;
using NetArchTest.Rules;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public partial class NamingConventionsTests
{
    [Fact]
    public void MiddlewareNames_ShouldEndWithMiddleware()
    {
        //Arrange
        var assembly = Shopway.Application.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IMiddleware))
            .Should()
            .HaveNameEndingWith(Middleware)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
