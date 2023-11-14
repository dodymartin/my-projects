using MinimalApi.Api.Features.Databases;

namespace MinimalApi.Api.Tests.ArchitectureTests.Databases;

public class NamespaceTests : BaseTest
{
    [Fact]
    public void Feature_Should_BeContained()
    {
        Features_Should_BeContained(typeof(Database).Namespace!);
    }
}
