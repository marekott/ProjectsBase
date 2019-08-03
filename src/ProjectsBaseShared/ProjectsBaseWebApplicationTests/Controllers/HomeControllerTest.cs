using System.Web.Mvc;
using NUnit.Framework;
using ProjectsBaseWebApplication.Controllers;
using Assert = NUnit.Framework.Assert;


namespace ProjectsBaseWebApplicationTests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
