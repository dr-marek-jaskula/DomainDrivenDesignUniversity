using NetArchTest.Rules;
using Shopway.Domain.BaseTypes;
using Shopway.Tests.Unit.ArchitectureTests.CustomRules;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.ArchitectureTests;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public sealed class EntityTests
{
    [Fact]
    public void Entities_ShouldDefineCreateMethod()
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(Entity<>))
            .And()
            .AreNotAbstract()
            .Should()
            .MeetCustomRule(new ContainsMethod("Create"))
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}