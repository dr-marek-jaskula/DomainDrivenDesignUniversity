using NetArchTest.Rules;
using Shopway.Tests.Unit.Constants;

namespace Shopway.Tests.Unit.ArchitectureTests;

[Trait(nameof(UnitTest), UnitTest.Architecture)]
public sealed class DependencyTests
{
    [Fact]
    public void Domain_ShouldNotHaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        var otherAssemblies = new[]
        {
            Application.AssemblyReference.Assembly.GetName().Name,
            Persistence.AssemblyReference.Assembly.GetName().Name,
            Infrastructure.AssemblyReference.Assembly.GetName().Name,
            Presentation.AssemblyReference.Assembly.GetName().Name,
            App.AssemblyReference.Assembly.GetName().Name,
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
        var assembly = Persistence.AssemblyReference.Assembly;

        var otherAssemblies = new[]
        {
            Application.AssemblyReference.Assembly.GetName().Name,
            Infrastructure.AssemblyReference.Assembly.GetName().Name,
            Presentation.AssemblyReference.Assembly.GetName().Name,
            App.AssemblyReference.Assembly.GetName().Name,
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
    public void Infrastructure_ShouldNotHaveDependencyOnOtherProjectsThanDomainAndPersistence()
    {
        //Arrange
        var assembly = Infrastructure.AssemblyReference.Assembly;

        var otherAssemblies = new[]
        {
            Application.AssemblyReference.Assembly.GetName().Name,
            Presentation.AssemblyReference.Assembly.GetName().Name,
            App.AssemblyReference.Assembly.GetName().Name,
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
    public void Application_ShouldNotHaveDependencyOnOtherProjectsThanDomainAndPersistenceAndInfrastructure()
    {
        //Arrange
        var assembly = Application.AssemblyReference.Assembly;

        var otherAssemblies = new[]
        {
            Presentation.AssemblyReference.Assembly.GetName().Name,
            App.AssemblyReference.Assembly.GetName().Name,
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
    public void Presentation_ShouldHaveDependencyDomainAndPersistenceAndInfrastructureAndApplication()
    {
        //Arrange
        var assembly = Presentation.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(App.AssemblyReference.Assembly.GetName().Name)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}