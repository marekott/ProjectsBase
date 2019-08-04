using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        [SetUp]
        public void Init()
        {
            _homeController = new HomeController();
        }

        [Test]
        public void IndexTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<BaseRepository<Project>>() //zamockowan metoda a wola sie normalna
                    .Setup(projectsRepository => projectsRepository.GetList())
                    .Returns(GetSampleProjects());

                var result = _homeController.Index();

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
    }
}
