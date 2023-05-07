using NetArchTest.Rules;
using Shopway.Persistence.Abstractions;
using static Shopway.Tests.Unit.Constants.NamingConvention;

namespace Shopway.Tests.Unit.SystemsUnderTest.Architecture.NamingConventions;

public partial class NamingConventionsTests
{
    [Fact]
    public void RepositoryNames_ShouldEndWithRepository()
    {
        //Arrange
        var assembly = Shopway.Persistence.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(RepositoryBase))
            .Should()
            .HaveNameEndingWith(Repository)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}