using learndotnetfast_web_services.Controllers;
using learndotnetfast_web_services.DTOs;
using learndotnetfast_web_services.Services.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace web_services_unit_tests.CourseModules
{
    [TestClass]
    public class CourseModuleControllerTests
    {
        private Mock<ICourseModuleService> _courseModuleServiceMock;
        private CourseModuleController _controller;

        public CourseModuleControllerTests()
        {
            _courseModuleServiceMock = new Mock<ICourseModuleService>();
            _controller = new CourseModuleController(null, _courseModuleServiceMock.Object);
        }

        [TestMethod]
        public async Task GetAllCourseModules_ReturnsOkResultWithLearningPathDTO()
        {
            // Arrange
            var learningPath = new LearningPathDTO
            {
                Modules = new List<CourseModuleDTO>()
            };
            _courseModuleServiceMock.Setup(service => service.GetAllCourseModules()).Returns(learningPath);

            // Act
            var result = await _controller.GetAllCourseModules();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(LearningPathDTO));
        }

        [TestMethod]
        public void GetAllCourseModulesAndTheirTutorials_ReturnsOkResultWithLearningPathDTO()
        {
            // Arrange
            var learningPath = new LearningPathDTO
            {
                Modules = new List<CourseModuleDTO>()
            };
            _courseModuleServiceMock.Setup(service => service.GetAllCourseModulesAndTheirTutorials()).Returns(learningPath);

            // Act
            var result = _controller.GetAllCourseModulesAndTheirTutorials();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(LearningPathDTO));
        }

        [TestMethod]
        public void CreateCourseModule_ValidModel_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var courseModuleDTO = new CourseModuleDTO { Id = 1 };
            _courseModuleServiceMock.Setup(service => service.SaveCourseModule(courseModuleDTO)).Returns(courseModuleDTO);

            // Act
            var result = _controller.CreateCourseModule(courseModuleDTO);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(nameof(CourseModuleController.GetCourseModuleById), createdAtActionResult.ActionName);
            Assert.AreEqual(courseModuleDTO.Id, ((CourseModuleDTO)createdAtActionResult.Value).Id);
        }

        [TestMethod]
        public void CreateCourseModule_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = _controller.CreateCourseModule(new CourseModuleDTO());

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void GetCourseModuleById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var courseModuleDTO = new CourseModuleDTO { Id = 1 };
            _courseModuleServiceMock.Setup(service => service.GetCourseModuleById(1)).Returns(courseModuleDTO);

            // Act
            var result = _controller.GetCourseModuleById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(courseModuleDTO, okResult.Value);
        }

        [TestMethod]
        public void GetCourseModuleById_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            _courseModuleServiceMock.Setup(service => service.GetCourseModuleById(1)).Returns((CourseModuleDTO)null);

            // Act
            var result = _controller.GetCourseModuleById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteCourseModuleById_ValidId_ReturnsOkResult()
        {
            // Act
            var result = _controller.DeleteCourseModuleById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
