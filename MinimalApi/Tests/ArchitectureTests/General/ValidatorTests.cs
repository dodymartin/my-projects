using System.Reflection;
using FluentAssertions;
using FluentValidation;
using MinimalApi.Api.Features.ApiCallUsages;
using NetArchTest.Rules;

namespace MinimalApi.Api.Tests.ArchitectureTests.Validators;

public class ValidatorTests
{
    private static readonly Assembly FeatureAssembly = typeof(ApiCallUsage).Assembly;

    [Fact]
    public void Handlers_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(FeatureAssembly)
            .That()
            .HaveNameEndingWith("Validator")
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_BeBasedOnAbstractValidator()
    {
        // Act
        var result = Types.InAssembly(FeatureAssembly)
            .That()
            .HaveNameEndingWith("Validator")
            .Should()
            .Inherit(typeof(AbstractValidator<>))
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
