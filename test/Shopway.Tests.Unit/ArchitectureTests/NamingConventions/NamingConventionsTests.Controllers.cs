using Microsoft.AspNetCore.Mvc;
using NetArchTest.Rules;
using Shopway.Tests.Unit.Constants;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

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