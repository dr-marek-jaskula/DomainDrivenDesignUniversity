using NetArchTest.Rules;
using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Tests.Unit.ArchitectureTests;

[UnitTest.Architecture]
public sealed class DomainEventTests
{
    [Fact]
    public void DomainEvents_ShouldBeSealed()
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        //Act
        var result = Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
