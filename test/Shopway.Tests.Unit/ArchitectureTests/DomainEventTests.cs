using NetArchTest.Rules;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.ArchitectureTests;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
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