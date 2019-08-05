using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac.Extras.Moq;
using NUnit.Framework;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;
using ProjectsBaseWebApplication.Controllers;
using Assert = NUnit.Framework.Assert;


namespace ProjectsBaseWebApplicationTests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        private HomeController _homeController;

        [Test]
        public void IndexTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<BaseRepository<Project>>()
                    .Setup(projectsRepository => projectsRepository.GetList())
                    .Returns(GetSampleProjects());

                _homeController = mock.Create<HomeController>();

                var result = _homeController.Index();

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public void IndexWithParameterNonExistingProjectTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<BaseRepository<Project>>()
                    .Setup(projectsRepository => projectsRepository.GetList())
                    .Returns(GetSampleProjects());

                _homeController = mock.Create<HomeController>();

                var result = _homeController.Index("Random name");

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public void IndexWithParameterExistingProjectTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<BaseRepository<Project>>()
                    .Setup(projectsRepository => projectsRepository.GetList())
                    .Returns(GetSampleProjects());

                _homeController = mock.Create<HomeController>();

                var result = _homeController.Index("Mocked project1");

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        private List<Project> GetSampleProjects()
        {
            return new List<Project>()
            {
                new Project()
                {
                    ProjectName = "Mocked project1"
                },
                new Project()
                {
                    ProjectName = "Mocked project2"
                }
            };
        }

        [Test]
        public void ProjectDetailsTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<BaseRepository<Project>>()
                    .Setup(projectsRepository => projectsRepository.Get(Guid.Empty, true))
                    .Returns(GetSampleProject());

                _homeController = mock.Create<HomeController>();

                var result = _homeController.ProjectDetails(Guid.NewGuid());

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public void ProjectDetailsPassNullTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<BaseRepository<Project>>()
                    .Setup(projectsRepository => projectsRepository.Get(Guid.Empty, true))
                    .Returns(GetSampleProject());

                _homeController = mock.Create<HomeController>();

                var result = _homeController.ProjectDetails(Guid.Empty);

                Assert.IsInstanceOf<HttpNotFoundResult>(result);
            }
        }

        private Project GetSampleProject()
        {
            return new Project()
            {
                ProjectName = "Mocked Project"
            };
        }
    }
}
