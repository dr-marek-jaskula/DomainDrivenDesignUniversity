﻿using Microsoft.EntityFrameworkCore;
using NetArchTest.Rules;
using static Shopway.Tests.Unit.Constants.Constants.NamingConvention;

namespace Shopway.Tests.Unit.ArchitectureTests.NamingConventions;

[UnitTest.Architecture]
public partial class NamingConventionsTests
{
    [Fact]
    public void ConfigurationNames_ShouldEndWithConfiguration()
    {
        //Arrange
        var assembly = Shopway.Persistence.AssemblyReference.Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .AreNotAbstract()
            .And()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .Should()
            .HaveNameEndingWith(Configuration)
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
