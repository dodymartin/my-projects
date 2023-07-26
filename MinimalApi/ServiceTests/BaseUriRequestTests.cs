using MinimalApi.Shared;

namespace ServiceTests
{
    public class BaseUriRequestTests
    {
        [Fact]
        public void GetBaseUriRequest_GoodIdGoodVersion_ReturnGood()
        {
            // Arrange
            var request = new BaseUriRequest()
            {
                ApplicationId = 1,
                ApplicationVersion = "4.1"
            };

            var validator = new BaseUriRequestValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void GetBaseUriRequest_BadIdGoodVersion_ReturnOneError()
        {
            // Arrange
            var request = new BaseUriRequest()
            {
                ApplicationId = null,
                ApplicationVersion = "4.1"
            };

            var validator = new BaseUriRequestValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.Single(result.Errors);
        }

        [Fact]
        public void GetBaseUriRequest_GoodIdBadVersion_ReturnOneError()
        {
            // Arrange
            var request = new BaseUriRequest()
            {
                ApplicationId = 1,
                ApplicationVersion = null
            };

            var validator = new BaseUriRequestValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.Single(result.Errors);
        }

        [Fact]
        public void GetBaseUriRequest_BadIdBadVersion_ReturnTwoErrors()
        {
            // Arrange
            var request = new BaseUriRequest()
            {
                ApplicationId = null,
                ApplicationVersion = null
            };

            var validator = new BaseUriRequestValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.Equal(2, result.Errors.Count);
        }
    }
}