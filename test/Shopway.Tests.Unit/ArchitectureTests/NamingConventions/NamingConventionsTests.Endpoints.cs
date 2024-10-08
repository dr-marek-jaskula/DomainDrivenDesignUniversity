﻿using FastEndpoints;
using NetArchTest.Rules;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[UnitTest.Architecture]
public partial class NamingConventionsTests
{
    [Fact]
    public void FastEndpointsNames_ShouldEndWithEndpoint()
    {
        //Arrange
        var assembly = Shopway.Presentation.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .AreClasses()
            .And()
            .Inherit(typeof(BaseEndpoint))
            .Should()
            .HaveNameEndingWith(Endpoint)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void MinimalEndpointsNames_ShouldEndWithEndpoint()
    {
        //Arrange
        var assembly = Shopway.Presentation.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(Shopway.Presentation.Abstractions.IEndpoint))
            .Should()
            .HaveNameEndingWith(Endpoint)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void MinimalEndpointGroupNames_ShouldEndWithGroup()
    {
        //Arrange
        var assembly = Shopway.Presentation.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(Shopway.Presentation.Abstractions.IEndpointGroup))
            .Should()
            .HaveNameEndingWith(NamingConvention.Group)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
