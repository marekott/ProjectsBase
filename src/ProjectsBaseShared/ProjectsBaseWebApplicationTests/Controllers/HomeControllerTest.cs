using System.Web.Mvc;
using NUnit.Framework;
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
            // Act
            var result = _homeController.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
