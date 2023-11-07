using NetArchTest.Rules;
using Shopway.Domain.BaseTypes;
using Shopway.Tests.Unit.ArchitectureTests.Utilities;
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
            .ContainMethod("Create")
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_ShouldHavePrivateParameterlessConstructor()
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        var result = Types.InAssembly(assembly)
            .That()
            .Inherit(typeof(Entity<>))
            .And()
            .AreNotAbstract()
            .Should()
            .HavePrivateParameterlessConstructor()
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}