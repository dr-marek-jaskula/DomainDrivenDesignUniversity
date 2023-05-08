using Quartz;
using NetArchTest.Rules;
using static Shopway.Tests.Unit.Constants.NamingConvention;
using Shopway.Tests.Unit.Constants;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

public partial class NamingConventionsTests
{
    [Fact]
    [Trait(TraitConstants.Category, TraitConstants.Architecture)]
    public void JobNames_ShouldEndWithJob()
    {
        //Arrange
        var assembly = Shopway.Infrastructure.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IJob))
            .Should()
            .HaveNameEndingWith(Job)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}