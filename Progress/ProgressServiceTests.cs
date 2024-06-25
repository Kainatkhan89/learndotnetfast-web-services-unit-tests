using AutoMapper;
using learndotnetfast_web_services.Data;
using learndotnetfast_web_services.DTOs;
using learndotnetfast_web_services.Repositories.Progress;
using learndotnetfast_web_services.Services.Progress;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;

namespace learndotnetfast_web_services.Tests.Progress
{
    public class ProgressServiceTests
    {
        private readonly Mock<IProgressRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProgressService _service;

        public ProgressServiceTests()
        {
            _mockRepo = new Mock<IProgressRepository>();
            _mockMapper = new Mock<IMapper>();
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDbForTesting")
                .Options;
            var context = new DataContext(options);

            _service = new ProgressService(_mockRepo.Object, _mockMapper.Object, context);
        }

        [Fact]
        public async Task AddCompletedTutorialAsync_ReturnsUpdatedProgress_WhenTutorialIsAdded()
        {
            // Arrange
            var userId = "testUser";
            var tutorialId = 1;
            var completionDTO = new TutorialCompletionDTO { UserId = userId, TutorialId = tutorialId };
            var progress = new Entities.Progress { UserId = userId, TutorialId = tutorialId };

            var progressList = new List<Entities.Progress> { progress };

            _mockRepo.Setup(x => x.GetProgressesByUserIdAsync(userId))
                .ReturnsAsync(progressList);

            _mockMapper.Setup(x => x.Map<ProgressDTO>(It.IsAny<Entities.Progress>()))
                .Returns(new ProgressDTO { UserId = userId, CompletedTutorialIds = new List<int> { tutorialId } });

            // Act
            var result = await _service.AddCompletedTutorialAsync(completionDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tutorialId, result.TutorialId);
            _mockRepo.Verify(x => x.AddProgressAsync(It.IsAny<Entities.Progress>()), Times.Once);
        }
    }

}
