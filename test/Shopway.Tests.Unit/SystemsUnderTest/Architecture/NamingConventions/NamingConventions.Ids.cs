using NetArchTest.Rules;
using Shopway.Domain.Abstractions;
using static Shopway.Tests.Unit.Constants.NamingConvention;

namespace Shopway.Tests.Unit.SystemsUnderTest.Architecture.NamingConventions;

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