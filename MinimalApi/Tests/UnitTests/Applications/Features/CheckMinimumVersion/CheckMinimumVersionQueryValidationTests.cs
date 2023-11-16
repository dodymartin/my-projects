using FluentAssertions.Execution;
using FluentAssertions;
using MinimalApi.Api.Features.Applications;

namespace MinimalApi.Api.Tests.UnitTests.Applications.Features.CheckMinimumVersion;

public class CheckMinimumVersionQueryValidationTests
{
    private readonly CheckMinimumVersionQueryValidator _validator = new();

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
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        };
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
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().HaveCount(3);
        };
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
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().HaveCount(2);
        };
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
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().ContainSingle();
        };
    }
}
