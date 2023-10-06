using NetArchTest.Rules;
using Shopway.Tests.Unit.Constants;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public partial class NamingConventionsTests
{
    [Fact]
    public void ExceptionNames_ShouldEndWithException()
    {
        //Arrange
        var assemblies = new[]
        {
            Shopway.Domain.AssemblyReference.Assembly,
            Shopway.Application.AssemblyReference.Assembly,
            Shopway.Persistence.AssemblyReference.Assembly,
            Shopway.Infrastructure.AssemblyReference.Assembly,
            Shopway.Presentation.AssemblyReference.Assembly,
            Shopway.App.AssemblyReference.Assembly,
        };

        //Act
        var result = Types
            .InAssemblies(assemblies)
            .That()
            .AreNotInterfaces()
            .And()
            .Inherit(typeof(Exception))
            .Should()
            .HaveNameEndingWith(NamingConvention.Exception)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}