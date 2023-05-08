using NetArchTest.Rules;
using Shopway.Persistence.Abstractions;
using Shopway.Tests.Unit.Constants;
using static Shopway.Tests.Unit.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

public partial class NamingConventionsTests
{
    [Fact]
    [Trait(TraitConstants.Category, TraitConstants.Architecture)]
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