using MinimalApi.Api.Features.Applications;

namespace MinimalApi.Api.Tests.IntegrationTests.Applications.Features.CheckMinimumVersion;

public class CheckMinimumVersionQueryValidationTests
{
    private CheckMinimumVersionQueryValidator _validator = new CheckMinimumVersionQueryValidator();

    [Fact]
    public void CheckMinimumVersionQuery_AllGoodData_ReturnValidWithNoErrors()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: 252,
            ApplicationName: "Name",
            ApplicationVersion: "5.2",
            FacilityId: 12
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
    public void CheckMinimumVersionQuery_AllBadData_ReturnInvalidWithTwoErrors()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: null,
            ApplicationName: "",
            ApplicationVersion: "",
            FacilityId: null
        );

        // Act
        var result = _validator.Validate(queryRequest);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(3, result.Errors.Count);
        });
    }

    [Fact]
    public void CheckMinimumVersionQuery_BadIdBadName_ReturnInvalidWithTwoErrors()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: null,
            ApplicationName: "",
            ApplicationVersion: "5.2",
            FacilityId: null
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

    [Fact]
    public void CheckMinimumVersionQuery_GoodIdGoodNameBadVersion_ReturnInvalidWithOneError()
    {
        // Arrange
        var queryRequest = new CheckMinimumVersionQuery(
            ApplicationId: 252,
            ApplicationName: "Name",
            ApplicationVersion: "",
            FacilityId: null
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
}
