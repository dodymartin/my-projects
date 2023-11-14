using MinimalApi.Api.Features.WebApis;

namespace MinimalApi.Api.Tests.ArchitectureTests.WebApis;

public class NamespaceTests : BaseTest
{
    [Fact]
    public void Feature_Should_BeContained()
    {
        Features_Should_BeContained(typeof(WebApi).Namespace!);
    }
}