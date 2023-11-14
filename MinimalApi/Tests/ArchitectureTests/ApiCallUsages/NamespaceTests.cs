using MinimalApi.Api.Features.ApiCallUsages;

namespace MinimalApi.Api.Tests.ArchitectureTests.ApiCallUsages;

public class NamespaceTests : BaseTest
{
    [Fact]
    public void Feature_Should_BeContained()
    {
        Features_Should_BeContained(typeof(ApiCallUsage).Namespace!);
    }
}
