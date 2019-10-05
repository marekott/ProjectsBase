using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
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

                var identity = new GenericIdentity("test_user");
                identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()));
                var principal = new GenericPrincipal(identity, new[] { "user" });

                var httpCtxStub = new Mock<HttpContextBase>();
                httpCtxStub.SetupGet(p => p.User).Returns(principal);
                var controllerCtx = new ControllerContext
                {
                    HttpContext = httpCtxStub.Object

                };

                _homeController = mock.Create<HomeController>();
                _homeController.ControllerContext = controllerCtx;

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

                var identity = new GenericIdentity("test_user");
                identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()));
                var principal = new GenericPrincipal(identity, new[] { "user" });

                var httpCtxStub = new Mock<HttpContextBase>();
                httpCtxStub.SetupGet(p => p.User).Returns(principal);
                var controllerCtx = new ControllerContext
                {
                    HttpContext = httpCtxStub.Object

                };

                _homeController = mock.Create<HomeController>();
                _homeController.ControllerContext = controllerCtx;

                var viewModel = new AddProjectViewModel()
                {
                    Project = new Project()
                };

                var result = _homeController.Add(viewModel);

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

                var identity = new GenericIdentity("test_user");
                identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()));
                var principal = new GenericPrincipal(identity, new[] { "user" });

                var httpCtxStub = new Mock<HttpContextBase>();
                httpCtxStub.SetupGet(p => p.User).Returns(principal);
                var controllerCtx = new ControllerContext
                {
                    HttpContext = httpCtxStub.Object

                };

                _homeController = mock.Create<HomeController>();
                _homeController.ModelState.AddModelError("key", "error message");
                _homeController.ControllerContext = controllerCtx;

                var viewModel = new AddProjectViewModel()
                {
                    Project = new Project()
                };

                var result = _homeController.Add(viewModel);

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

                var identity = new GenericIdentity("test_user");
                identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()));
                var principal = new GenericPrincipal(identity, new[] { "user" });

                var httpCtxStub = new Mock<HttpContextBase>();
                httpCtxStub.SetupGet(p => p.User).Returns(principal);
                var controllerCtx = new ControllerContext
                {
                    HttpContext = httpCtxStub.Object

                };

                _homeController = mock.Create<HomeController>();
                _homeController.ControllerContext = controllerCtx;

                var viewModel = new AddProjectViewModel()
                {
                    Project = new Project()
                };

                var result = _homeController.Add(viewModel);

                var routeResult = result as RedirectToRouteResult;
                Assert.AreEqual("Index", (string)routeResult?.RouteValues["action"]);
            }
        }

        [Test]
        public void MyProjectsTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Project>>()
                    .Setup(projectsRepository => projectsRepository.GetList())
                    .Returns(GetSampleProjects());

                var identity = new GenericIdentity("test_user");
                identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()));
                var principal = new GenericPrincipal(identity, new[] { "user" });

                var httpCtxStub = new Mock<HttpContextBase>();
                httpCtxStub.SetupGet(p => p.User).Returns(principal);
                var controllerCtx = new ControllerContext
                {
                    HttpContext = httpCtxStub.Object

                };

                _homeController = mock.Create<HomeController>();
                _homeController.ControllerContext = controllerCtx;

                var result = _homeController.MyProjects();

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }
    }
}
