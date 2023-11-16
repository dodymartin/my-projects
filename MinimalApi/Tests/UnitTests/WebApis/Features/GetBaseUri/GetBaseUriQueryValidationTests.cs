using FluentAssertions.Execution;
using FluentAssertions;
using MinimalApi.Api.Features.WebApis;

namespace MinimalApi.Api.Tests.UnitTests.WebApis.Features.GetBaseUri;

public class GetBaseUriQueryValidationTests
{
    private readonly GetBaseUriQueryValidator _validator = new();

    [Fact]
    public void GetBaseUriQuery_GoodIdGoodVersion_ReturnValidWithNoErrors()
    {
        // Arrange
        var queryRequest = new GetBaseUriQuery(
            ApplicationId: 252,
            ApplicationVersion: "5.2"
        );

        // Act
        var result = _validator.Validate(queryRequest);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        };
    }

    [Fact]
    public void GetBaseUriQuery_GoodIdBadVersion_ReturnInvalidWithOneError()
    {
        // Arrange
        var queryRequest = new GetBaseUriQuery(
            ApplicationId: 252,
            ApplicationVersion: ""
        );

        // Act
        var result = _validator.Validate(queryRequest);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().ContainSingle();
        };
    }

    [Fact]
    public void GetBaseUriQuery_BadIdGoodVersion_ReturnInvalidWithOneError()
    {
        // Arrange
        var queryRequest = new GetBaseUriQuery(
            ApplicationId: null,
            ApplicationVersion: "5.2"
        );

        // Act
        var result = _validator.Validate(queryRequest);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().ContainSingle();
        };
    }

    [Fact]
    public void GetBaseUriQuery_BadIdBadVersion_ReturnInvalidWithTwoErrors()
    {
        // Arrange
        var queryRequest = new GetBaseUriQuery(
            ApplicationId: null,
            ApplicationVersion: ""
        );

        // Act
        var result = _validator.Validate(queryRequest);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().HaveCount(2);
        };
    }
}
