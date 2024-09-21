using Microsoft.AspNetCore.Mvc;
using NetArchTest.Rules;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[UnitTest.Architecture]
public partial class NamingConventionsTests
{
    [Fact]
    public void ControllerNames_ShouldEndWithController()
    {
        //Arrange
        var assembly = Shopway.Presentation.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(ControllerBase))
            .Should()
            .HaveNameEndingWith(NamingConvention.Controller)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
