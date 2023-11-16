using System.Reflection;
using FluentAssertions;
using MediatR;
using MinimalApi.Api.Features.ApiCallUsages;
using NetArchTest.Rules;

namespace MinimalApi.Api.Tests.ArchitectureTests.Queries;

public class QueryTests
{
    private static readonly Assembly FeatureAssembly = typeof(ApiCallUsage).Assembly;

    [Fact]
    public void Queries_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(FeatureAssembly)
            .That()
            .HaveNameEndingWith("Query")
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Queries_Implements_IRequestErrorOr()
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
