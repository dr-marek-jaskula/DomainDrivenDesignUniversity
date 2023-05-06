using NetArchTest.Rules;
using Shopway.Domain.Abstractions;
using static Shopway.Tests.Unit.Constants.NamingConventions;

namespace Shopway.Tests.Unit.SystemsUnderTest.Architecture.NamingConventions;

public partial class NamingConventionsTests
{
    [Fact]
    public void DomainEventNames_ShouldEndWithDomainEvent()
    {
        //Arrange
        var assembly = Shopway.Application.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith(DomainEvent)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}