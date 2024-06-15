using NetArchTest.Rules;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public partial class NamingConventionsTests
{
    [Fact]
    public void EntityKeyNames_ShouldEndWithKey()
    {
        //Arrange
        var assembly = Shopway.Domain.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .AreNotInterfaces()
            .And()
            .ImplementInterface(typeof(IUniqueKey))
            .Should()
            .HaveNameEndingWith(Key)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
