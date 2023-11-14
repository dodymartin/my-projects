using System.Reflection;
using Carter;
using FluentAssertions;
using MinimalApi.Api.Features.ApiCallUsages;
using NetArchTest.Rules;

namespace MinimalApi.Api.Tests.ArchitectureTests.Endpoints;

public class EndpointTests
{
    private static readonly Assembly FeatureAssembly = typeof(ApiCallUsage).Assembly;

    [Fact]
    public void Endpoints_Should_ImplementCarter()
    {
        // Act
        var result = Types.InAssembly(FeatureAssembly)
            .That()
            .HaveNameEndingWith("Endpoint")
            .Should()
            .ImplementInterface(typeof(ICarterModule))
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Endpoints_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(FeatureAssembly)
            .That()
            .HaveNameEndingWith("Endpoint")
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Endpoints_Should_HaveEndpointPostfix()
    {
        // Act
        var result = Types.InAssembly(FeatureAssembly)
            .That()
            .ImplementInterface(typeof(ICarterModule))
            .Should()
            .HaveNameEndingWith("Endpoint")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
