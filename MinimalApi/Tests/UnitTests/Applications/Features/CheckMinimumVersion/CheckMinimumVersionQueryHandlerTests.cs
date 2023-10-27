using MinimalApi.Api.Features.Applications;
using NSubstitute;
using ApplicationId = MinimalApi.Api.Features.Applications.ApplicationId;

namespace MinimalApi.Api.Tests.UnitTests.Applications.Features.CheckMinimumVersion;

public class CheckMinimumVersionQueryHandlerTests
{
    private IApplicationRepo _mockRepo;
    private CheckMinimumVersionQueryHandler _handler;

    public CheckMinimumVersionQueryHandlerTests()
    {
        _mockRepo = Substitute.For<IApplicationRepo>();
        _handler = new CheckMinimumVersionQueryHandler(_mockRepo);
    }

    [Fact]
    public async Task CheckMinimumVersionQuery_FoundApplicationGoodVersion_ReturnTrue()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: 252,
            ApplicationName: "",
            ApplicationVersion: "5.2",
            FacilityId: null
        );

        var application = new Application
        {
            Id = ApplicationId.Create(queryRequest.ApplicationId!.Value),
            MinimumAssemblyVersion = "5.2",
        };

        _mockRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, CancellationToken.None)
            .Returns(application);
        _mockRepo.GetApplicationAsync(queryRequest.ApplicationName!, CancellationToken.None)
            .Returns(application);

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsError);
            Assert.True(result.Value);
        });
    }

    [Fact]
    public async Task CheckMinimumVersionQuery_FoundApplicationGoodVersionByFacility_ReturnTrue()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: 252,
            ApplicationName: "",
            ApplicationVersion: "5.2",
            FacilityId: 12
        );

        var application = new Application
        {
            Id = ApplicationId.Create(queryRequest.ApplicationId!.Value),
            MinimumAssemblyVersion = "",
        };

        _mockRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, CancellationToken.None)
            .Returns(application);
        _mockRepo.GetApplicationAsync(queryRequest.ApplicationName!, CancellationToken.None)
            .Returns(application);
        _mockRepo.GetMinimumVersionAsync(queryRequest.ApplicationId!.Value, queryRequest.FacilityId!.Value, CancellationToken.None)
            .Returns("5.2");

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsError);
            Assert.True(result.Value);
        });
    }

    [Fact]
    public async Task CheckMinimumVersionQuery_FoundApplicationBadVersion_ReturnFalse()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: 252,
            ApplicationName: "",
            ApplicationVersion: "5.2",
            FacilityId: null
        );

        var application = new Application
        {
            Id = ApplicationId.Create(queryRequest.ApplicationId!.Value),
            MinimumAssemblyVersion = "5.2.1",
        };

        _mockRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, CancellationToken.None)
            .Returns(application);
        _mockRepo.GetApplicationAsync(queryRequest.ApplicationName!, CancellationToken.None)
            .Returns(application);

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsError);
            Assert.False(result.Value);
        });
    }

    [Fact]
    public async Task CheckMinimumVersionQuery_FoundApplicationBadVersionByFacility_ReturnFalse()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: 252,
            ApplicationName: "",
            ApplicationVersion: "5.2",
            FacilityId: 12
        );

        var application = new Application
        {
            Id = ApplicationId.Create(queryRequest.ApplicationId!.Value),
            MinimumAssemblyVersion = ""
        };

        _mockRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, CancellationToken.None)
            .Returns(application);
        _mockRepo.GetApplicationAsync(queryRequest.ApplicationName!, CancellationToken.None)
            .Returns(application);
        _mockRepo.GetMinimumVersionAsync(queryRequest.ApplicationId!.Value, queryRequest.FacilityId!.Value, CancellationToken.None)
            .Returns("5.2.1");

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsError);
            Assert.False(result.Value);
        });
    }

    [Fact]
    public async Task CheckMinimumVersionQuery_NotFoundApplication_ReturnOneError()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: 252,
            ApplicationName: "",
            ApplicationVersion: "5.2",
            FacilityId: null
        );

        _mockRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, CancellationToken.None)
            .Returns(default(Application));

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsError);
            Assert.NotEmpty(result.Errors);
            Assert.Single(result.Errors);
        });
    }
}
