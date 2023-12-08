using System.Reflection;
using FluentAssertions;
using MediatR;
using MinimalApi.Api.Features.ApiCallUsages;
using NetArchTest.Rules;

namespace MinimalApi.Api.Tests.ArchitectureTests.Handlers;

public class HandlerTests
{
    private static readonly Assembly _featureAssembly = typeof(ApiCallUsage).Assembly;

    [Fact]
    public void Handlers_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(_featureAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Implements_IRequestHandler()
    {
        // Act
        var result = Types.InAssembly(_featureAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .ImplementInterface(typeof(IRequestHandler<,>))
            //.Or()
            //.ImplementInterface(typeof(IExceptionHandler))
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
