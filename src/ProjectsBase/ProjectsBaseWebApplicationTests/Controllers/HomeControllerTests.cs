using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;
using ProjectsBaseWebApplication.Controllers;
using ProjectsBaseWebApplication.Models;
using ProjectsBaseWebApplication.ViewModels;
using Assert = NUnit.Framework.Assert;


namespace ProjectsBaseWebApplicationTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _homeController;

        [Test]
        public void IndexTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Project>>()
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
                mock.Mock<IRepository<Project>>()
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
                mock.Mock<IRepository<Project>>()
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
                mock.Mock<IRepository<Project>>()
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
                mock.Mock<IRepository<Project>>()
                    .Setup(projectsRepository => projectsRepository.Get(Guid.Empty, true))
                    .Returns(GetSampleProject());
                _homeController = mock.Create<HomeController>();

                var result = _homeController.ProjectDetails(null);

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

        [Test]
        public void AddTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Client>>()
                    .Setup(c => c.GetList())
                    .Returns(GetSampleClients());
                _homeController = mock.Create<HomeController>();

                var result = _homeController.Add();

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public void AddInvalidProjectTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IValidator<Project>>()
                    .Setup(v => v.Validate(It.IsAny<Project>()))
                    .Returns(false);
                mock.Mock<IRepository<Client>>()
                    .Setup(c => c.GetList())
                    .Returns(GetSampleClients());
                _homeController = mock.Create<HomeController>();

                var result = _homeController.Add(new AddProjectViewModel());

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public void AddProjectModelStateErrorTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Client>>()
                    .Setup(c => c.GetList())
                    .Returns(GetSampleClients());
                _homeController = mock.Create<HomeController>();
                _homeController = mock.Create<HomeController>();
                _homeController.ModelState.AddModelError("key", "error message");

                var result = _homeController.Add(new AddProjectViewModel());

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        private List<Client> GetSampleClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = Guid.NewGuid()
                },
                new Client()
                {
                    ClientId = Guid.NewGuid()
                }
            };
        }

        [Test]
        public void AddValidProjectTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IValidator<Project>>()
                    .Setup(v => v.Validate(It.IsAny <Project>()))
                    .Returns(true);
                _homeController = mock.Create<HomeController>();

                var result = _homeController.Add(new AddProjectViewModel());

                var routeResult = result as RedirectToRouteResult;
                Assert.AreEqual("Index", (string)routeResult?.RouteValues["action"]);
            }
        }
    }
}
