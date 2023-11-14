using MinimalApi.Api.Features.Applications;

namespace MinimalApi.Api.Tests.ArchitectureTests.Applications;

public class NamespaceTests : BaseTest
{
    [Fact]
    public void Feature_Should_BeContained()
    {
        Features_Should_BeContained(typeof(Application).Namespace!);
    }
}
