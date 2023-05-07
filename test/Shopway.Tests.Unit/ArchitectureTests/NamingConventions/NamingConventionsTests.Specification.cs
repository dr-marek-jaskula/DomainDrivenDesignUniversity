using NetArchTest.Rules;
using Shopway.Persistence.Abstractions;
using static Shopway.Tests.Unit.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

public partial class NamingConventionsTests
{
    [Fact]
    public void SpecificationNames_ShouldConatinSpecification()
    {
        //Arrange
        var assembly = Shopway.Persistence.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(SpecificationBase<,>))
            .Should()
            .HaveNameMatching(Specification)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}