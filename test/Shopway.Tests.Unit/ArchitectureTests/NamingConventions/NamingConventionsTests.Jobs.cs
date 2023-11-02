using Quartz;
using NetArchTest.Rules;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public partial class NamingConventionsTests
{
    [Fact]
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