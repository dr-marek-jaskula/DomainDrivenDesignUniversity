using Shopway.Domain.Common.Utilities;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[UnitTest.Architecture]
public partial class NamingConventionsTests
{
    [Fact]
    public void RepositoryNames_ShouldEndWithRepository()
    {
        //Arrange
        var assembly = Shopway.Persistence.AssemblyReference.Assembly;

        //Act
        var repositories = assembly
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(x => x.Name.EndsWith(Repository)));

        //Assert
        repositories
            .Should()
            .OnlyContain(x => x.Name.EndsWith(Repository));
    }
}
