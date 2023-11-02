using FluentValidation;
using NetArchTest.Rules;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public partial class NamingConventionsTests
{
    [Fact]
    public void ValidatorsNames_ShouldEndWithValidators()
    {
        //Arrange
        var assembly = Shopway.Application.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .AreNotAbstract()
            .And()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith(Validator)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}