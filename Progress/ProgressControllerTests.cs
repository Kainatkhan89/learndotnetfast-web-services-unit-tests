using learndotnetfast_web_services.Controllers;
using learndotnetfast_web_services.DTOs;
using learndotnetfast_web_services.Services.Progress;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace learndotnetfast_web_services.Tests.Progress
{
    public class ProgressControllerTests
    {
        private readonly Mock<IProgressService> _mockService;
        private readonly ProgressController _controller;

        public ProgressControllerTests()
        {
            _mockService = new Mock<IProgressService>();
            _controller = new ProgressController(_mockService.Object);
        }

        [Fact]
        public async Task GetProgress_ReturnsNotFound_WhenProgressIsNotAvailable()
        {
            // Arrange
            var userId = "testUser";
            _mockService.Setup(x => x.GetProgressByUserIdAsync(userId))
                .ReturnsAsync((ProgressDTO)null);

            // Act
            var result = await _controller.GetProgress(userId);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
        }

        [Fact]
        public async Task GetProgress_ReturnsProgress_WhenProgressIsAvailable()
        {
            // Arrange
            var userId = "testUser";
            var progressDto = new ProgressDTO { UserId = userId, CompletedTutorialIds = new List<int> { 1, 2 } };
            _mockService.Setup(x => x.GetProgressByUserIdAsync(userId))
                .ReturnsAsync(progressDto);

            // Act
            var result = await _controller.GetProgress(userId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as ProgressDTO;
            Assert.IsNotNull(returnValue);
            Assert.Equals(userId, returnValue.UserId);
        }
    }
}
