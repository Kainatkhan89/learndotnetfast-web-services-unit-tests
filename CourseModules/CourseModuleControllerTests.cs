using AutoFixture;
using learndotnetfast_web_services.Controllers;
using learndotnetfast_web_services.Repositories.CourseModule;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_services_unit_tests.CourseModules
{
    [TestClass]
    class CourseModuleControllerTests
    {

        private Mock<ICourseModuleRepository> _courseModuleRepositoryMock;
        private Fixture _fixture;
        private CourseModuleController _controller;

        public CourseModuleControllerTests()
        {
            _courseModuleRepositoryMock = new Mock<ICourseModuleRepository>();
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_CourseModule_ResultOk()
        {

        }


    }
}
