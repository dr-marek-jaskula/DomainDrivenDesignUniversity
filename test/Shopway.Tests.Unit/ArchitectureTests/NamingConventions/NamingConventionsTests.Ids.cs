using NetArchTest.Rules;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public partial class NamingConventionsTests
{
    [Fact]
    public void EntityIdNames_ShouldEndWithId()
    {
        //Arrange
        var assembly = Shopway.Domain.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .AreNotInterfaces()
            .And()
            .ImplementInterface(typeof(IEntityId))
            .Should()
            .HaveNameEndingWith(Id)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}