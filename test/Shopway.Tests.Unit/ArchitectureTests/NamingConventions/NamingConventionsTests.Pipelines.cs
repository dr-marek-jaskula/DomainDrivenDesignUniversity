using MediatR;
using NetArchTest.Rules;
using Shopway.Tests.Unit.Constants;
using static Shopway.Tests.Unit.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

public partial class NamingConventionsTests
{
    [Fact]
    [Trait(TraitConstants.Category, TraitConstants.Architecture)]
    public void PipelineNames_ShouldContainPipeline()
    {
        //Arrange
        var assembly = Shopway.Application.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IPipelineBehavior<,>))
            .Should()
            .HaveNameMatching(Pipeline)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}