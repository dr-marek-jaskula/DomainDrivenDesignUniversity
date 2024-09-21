using NetArchTest.Rules;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Tests.Unit.ArchitectureTests.Utilities;

namespace Shopway.Tests.Unit.ArchitectureTests;

[UnitTest.Architecture]
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
            .DefineMethod(IEntity.CreateMethodName)
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
