using System.Reflection;
using FluentAssertions;
using MediatR;
using MinimalApi.Api.Features.ApiCallUsages;
using NetArchTest.Rules;

namespace MinimalApi.Api.Tests.ArchitectureTests.Commands;

public class CommandTests
{
    private static readonly Assembly FeatureAssembly = typeof(ApiCallUsage).Assembly;

    [Fact]
    public void Commands_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(FeatureAssembly)
            .That()
            .HaveNameEndingWith("Command")
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_Implements_IRequestErrorOr()
    {
        // Act
        var result = Types.InAssembly(FeatureAssembly)
            .That()
            .HaveNameEndingWith("Query")
            .Should()
            .ImplementInterface(typeof(IRequest<>))
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
