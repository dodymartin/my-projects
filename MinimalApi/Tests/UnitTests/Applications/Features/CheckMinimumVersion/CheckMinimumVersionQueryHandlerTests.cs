using FluentAssertions.Execution;
using FluentAssertions;
using MinimalApi.Api.Features.Applications;
using NSubstitute;
using ApplicationId = MinimalApi.Api.Features.Applications.ApplicationId;

namespace MinimalApi.Api.Tests.UnitTests.Applications.Features.CheckMinimumVersion;

public class CheckMinimumVersionQueryHandlerTests
{
    private readonly IApplicationRepo _mockRepo;
    private readonly CheckMinimumVersionQueryHandler _handler;

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
            ExeName = "",
            FromDirectoryName = "",
            Name = "",
            MinimumAssemblyVersion = "5.2",
        };

        _mockRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, CancellationToken.None)
            .Returns(application);
        _mockRepo.GetApplicationAsync(queryRequest.ApplicationName!, CancellationToken.None)
            .Returns(application);

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.IsError.Should().BeFalse();
            result.Value.Should().BeTrue();
        };
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
            ExeName = "",
            FromDirectoryName = "",
            Name = "",
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
        using (new AssertionScope())
        {
            result.IsError.Should().BeFalse();
            result.Value.Should().BeTrue();
        };
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
            ExeName = "",
            FromDirectoryName = "",
            Name = "",
            MinimumAssemblyVersion = "5.2.1",
        };

        _mockRepo.GetApplicationAsync(queryRequest.ApplicationId!.Value, CancellationToken.None)
            .Returns(application);
        _mockRepo.GetApplicationAsync(queryRequest.ApplicationName!, CancellationToken.None)
            .Returns(application);

        // Act
        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.IsError.Should().BeFalse();
            result.Value.Should().BeFalse();
        };
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
            ExeName = "",
            FromDirectoryName = "",
            Name = "",
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
        using (new AssertionScope())
        {
            result.IsError.Should().BeFalse();
            result.Value.Should().BeFalse();
        };
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
        using (new AssertionScope())
        {
            result.IsError.Should().BeTrue();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().ContainSingle();
        };
    }
}
