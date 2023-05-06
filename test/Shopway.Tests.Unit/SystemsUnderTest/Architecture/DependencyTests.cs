using NetArchTest.Rules;

namespace Shopway.Tests.Unit.SystemsUnderTest.Architecture;

public sealed class DependencyTests
{
    [Fact]
    public void Domain_ShouldNotHaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = Shopway.Domain.AssemblyReference.Assembly;

        var otherAssemblies = new[]
        {
            Shopway.Application.AssemblyReference.Assembly.GetName().Name,
            Shopway.Persistence.AssemblyReference.Assembly.GetName().Name,
            Shopway.Infrastructure.AssemblyReference.Assembly.GetName().Name,
            Shopway.Presentation.AssemblyReference.Assembly.GetName().Name,
            Shopway.App.AssemblyReference.Assembly.GetName().Name,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherAssemblies)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Persistence_ShouldNotHaveDependencyOnOtherProjectsThanDomain()
    {
        //Arrange
        var assembly = Shopway.Persistence.AssemblyReference.Assembly;

        var otherAssemblies = new[]
        {
            Shopway.Application.AssemblyReference.Assembly.GetName().Name,
            Shopway.Infrastructure.AssemblyReference.Assembly.GetName().Name,
            Shopway.Presentation.AssemblyReference.Assembly.GetName().Name,
            Shopway.App.AssemblyReference.Assembly.GetName().Name,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherAssemblies)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldNotHaveDependencyOnOtherProjectsThanDomainAndPersistance()
    {
        //Arrange
        var assembly = Shopway.Infrastructure.AssemblyReference.Assembly;

        var otherAssemblies = new[]
        {
            Shopway.Application.AssemblyReference.Assembly.GetName().Name,
            Shopway.Presentation.AssemblyReference.Assembly.GetName().Name,
            Shopway.App.AssemblyReference.Assembly.GetName().Name,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherAssemblies)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_ShouldNotHaveDependencyOnOtherProjectsThanDomainAndPersistanceAndInfrastructure()
    {
        //Arrange
        var assembly = Shopway.Application.AssemblyReference.Assembly;

        var otherAssemblies = new[]
        {
            Shopway.Presentation.AssemblyReference.Assembly.GetName().Name,
            Shopway.App.AssemblyReference.Assembly.GetName().Name,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherAssemblies)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Presentation_ShouldHaveDependencyDomainAndPersistanceAndInfrastructureAndApplication()
    {
        //Arrange
        var assembly = Shopway.Presentation.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(Shopway.App.AssemblyReference.Assembly.GetName().Name)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}