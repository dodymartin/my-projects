using System.Reflection;
using FluentAssertions;
using MinimalApi.Api.Features.ApiCallUsages;
using NetArchTest.Rules;

namespace MinimalApi.Api.Tests.ArchitectureTests
{
    public abstract class BaseTest
    {
        protected static readonly Assembly FeatureAssembly = typeof(ApiCallUsage).Assembly;
        protected static readonly string GeneralFeatureNamespace = "MinimalApi.Api.Features";

        protected void Features_Should_BeContained(string featureNamespace)
        {
            // Act
            var result = Types.InAssembly(FeatureAssembly)
                .That()
                .ResideInNamespace(featureNamespace)
                .Should()
                .NotHaveDependencyOnAny(
                    Types.InAssembly(FeatureAssembly)
                        .That()
                        .DoNotResideInNamespace(featureNamespace)
                        .And()
                        .ResideInNamespaceStartingWith(GeneralFeatureNamespace)
                        .GetTypes()
                    .Select(n => n.Namespace)
                    .ToArray())
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
