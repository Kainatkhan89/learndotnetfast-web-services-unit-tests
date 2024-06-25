using AutoFixture;
using AutoMapper;
using learndotnetfast_web_services.Common.Exceptions.Custom;
using learndotnetfast_web_services.DTOs;
using learndotnetfast_web_services.Entities;
using learndotnetfast_web_services.Repositories.CourseModule;
using learndotnetfast_web_services.Services.Courses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_services_unit_tests.CourseModules
{
    [TestClass]
    public class CourseModuleServiceTests
    {
        private Mock<ICourseModuleRepository> _courseModuleRepositoryMock;
        private Fixture _fixture;
        private CourseModuleService _service;
        private IMapper _mapper;

        public CourseModuleServiceTests()
        {
            _fixture = new Fixture();
            _courseModuleRepositoryMock = new Mock<ICourseModuleRepository>();

            // Replace ThrowingRecursionBehavior with OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Initialize AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseModule, CourseModuleDTO>();
                cfg.CreateMap<CourseModuleDTO, CourseModule>();
                cfg.CreateMap<Tutorial, TutorialDTO>();
                cfg.CreateMap<TutorialDTO, Tutorial>();
                // Add other mappings if needed
            });
            _mapper = config.CreateMapper();
        }


        [TestMethod]
        public void GetCourseModuleById_success()
        {
            // Arrange
            var courseModule = _fixture.Create<CourseModule>();

            _courseModuleRepositoryMock.Setup(repo => repo.FindById(courseModule.Id))
                .Returns(courseModule);

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act
            var resultDTO = _service.GetCourseModuleById(courseModule.Id);

            // Map expected result
            var expectedDTO = _mapper.Map<CourseModuleDTO>(courseModule);

            // Assert
            Assert.IsNotNull(resultDTO);
            Assert.AreEqual(expectedDTO.Id, resultDTO.Id);
            Assert.AreEqual(expectedDTO.Title, resultDTO.Title);
            // Add other property assertions as needed
        }

        [TestMethod]
        public void GetCourseModuleById_notFound()
        {
            // Arrange
            var courseId = _fixture.Create<int>();

            _courseModuleRepositoryMock.Setup(repo => repo.FindById(courseId))
                .Returns((CourseModule)null);

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act & Assert
            Assert.ThrowsException<ResourceNotFoundException>(() => _service.GetCourseModuleById(courseId));
        }

        [TestMethod]
        public void SaveCourseModule_success()
        {
            // Arrange
            var courseModule = _fixture.Create<CourseModule>();
            var courseModuleDTO = _mapper.Map<CourseModuleDTO>(courseModule);
            var savedCourseModule = _fixture.Create<CourseModule>();

            _courseModuleRepositoryMock.Setup(repo => repo.ExistsByTitle(courseModuleDTO.Title)).Returns(false);
            _courseModuleRepositoryMock.Setup(repo => repo.ExistsByNumber(courseModuleDTO.Number)).Returns(false);
            _courseModuleRepositoryMock.Setup(repo => repo.Save(It.IsAny<CourseModule>())).Returns(savedCourseModule);

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act
            var resultDTO = _service.SaveCourseModule(courseModuleDTO);

            // Assert
            Assert.IsNotNull(resultDTO);
            Assert.AreEqual(savedCourseModule.Id, resultDTO.Id);
            // Add other property assertions as needed
        }

        [TestMethod]
        public void SaveCourseModule_duplicateTitle()
        {
            // Arrange
            var courseModuleDTO = _fixture.Create<CourseModuleDTO>();

            _courseModuleRepositoryMock.Setup(repo => repo.ExistsByTitle(courseModuleDTO.Title)).Returns(true);

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act & Assert
            Assert.ThrowsException<DuplicateResourceException>(() => _service.SaveCourseModule(courseModuleDTO));
        }

        [TestMethod]
        public void SaveCourseModule_duplicateNumber()
        {
            // Arrange
            var courseModuleDTO = _fixture.Create<CourseModuleDTO>();

            _courseModuleRepositoryMock.Setup(repo => repo.ExistsByNumber(courseModuleDTO.Number)).Returns(true);

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act & Assert
            Assert.ThrowsException<DuplicateResourceException>(() => _service.SaveCourseModule(courseModuleDTO));
        }

        [TestMethod]
        public void DeleteCourseModuleById_success()
        {
            // Arrange
            var courseId = _fixture.Create<int>();

            _courseModuleRepositoryMock.Setup(repo => repo.ExistsById(courseId)).Returns(true);
            _courseModuleRepositoryMock.Setup(repo => repo.DeleteById(courseId));

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act
            _service.DeleteCourseModuleById(courseId);

            // Assert
            _courseModuleRepositoryMock.Verify(repo => repo.DeleteById(courseId), Times.Once);
        }

        [TestMethod]
        public void DeleteCourseModuleById_notFound()
        {
            // Arrange
            var courseId = _fixture.Create<int>();

            _courseModuleRepositoryMock.Setup(repo => repo.ExistsById(courseId)).Returns(false);

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act & Assert
            Assert.ThrowsException<ResourceNotFoundException>(() => _service.DeleteCourseModuleById(courseId));
        }

        [TestMethod]
        public void GetAllCourseModules_success()
        {
            // Arrange
            var courseModules = _fixture.CreateMany<CourseModule>().ToList();

            _courseModuleRepositoryMock.Setup(repo => repo.FindAll()).Returns(courseModules);

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act
            var resultDTO = _service.GetAllCourseModules();

            // Map expected result
            var expectedDTO = new LearningPathDTO
            {
                Modules = _mapper.Map<List<CourseModuleDTO>>(courseModules)
            };

            // Assert
            Assert.IsNotNull(resultDTO);
            Assert.AreEqual(expectedDTO.Modules.Count, resultDTO.Modules.Count);
            // Add other property assertions as needed
        }

        [TestMethod]
        public void GetAllCourseModulesAndTheirTutorials_success()
        {
            // Arrange
            var courseModules = _fixture.CreateMany<CourseModule>().ToList();

            _courseModuleRepositoryMock.Setup(repo => repo.FindAllAndTheirTutorials()).Returns(courseModules);

            _service = new CourseModuleService(_courseModuleRepositoryMock.Object, _mapper);

            // Act
            var resultDTO = _service.GetAllCourseModulesAndTheirTutorials();

            // Map expected result
            var expectedDTO = new LearningPathDTO
            {
                Modules = _mapper.Map<List<CourseModuleDTO>>(courseModules)
            };

            // Assert
            Assert.IsNotNull(resultDTO);
            Assert.AreEqual(expectedDTO.Modules.Count, resultDTO.Modules.Count);
            // Add other property assertions as needed
        }
    }
}
