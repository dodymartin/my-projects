using System.Reflection;
using FluentAssertions;
using MinimalApi.Api.Features.ApiCallUsages;
using NetArchTest.Rules;

namespace MinimalApi.Api.Tests.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly _featureAssembly = typeof(ApiCallUsage).Assembly;
    protected static readonly string _generalFeatureNamespace = "MinimalApi.Api.Features";

    protected static void Features_Should_BeContained(string featureNamespace)
    {
        // Act
        var result = Types.InAssembly(_featureAssembly)
            .That()
            .ResideInNamespace(featureNamespace)
            .Should()
            .NotHaveDependencyOnAny(
                Types.InAssembly(_featureAssembly)
                    .That()
                    .DoNotResideInNamespace(featureNamespace)
                    .And()
                    .ResideInNamespaceStartingWith(_generalFeatureNamespace)
                    .GetTypes()
                .Select(n => n.Namespace)
                .ToArray())
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
