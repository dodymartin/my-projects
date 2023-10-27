using MinimalApi.Api.Features.WebApis;

namespace MinimalApi.Api.Tests.IntegrationTests.WebApis.Features.GetBaseUri;

public class GetBaseUriQueryValidationTests
{
    private GetBaseUriQueryValidator _validator = new GetBaseUriQueryValidator();

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
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        });
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
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Single(result.Errors);
        });
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
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Single(result.Errors);
        });
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
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(2, result.Errors.Count);
        });
    }
}
