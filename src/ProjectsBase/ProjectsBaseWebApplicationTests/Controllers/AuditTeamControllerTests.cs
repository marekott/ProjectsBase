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
using Assert = NUnit.Framework.Assert;

namespace ProjectsBaseWebApplicationTests.Controllers
{
    [TestFixture]
    public class AuditTeamControllerTests
    {
        private AuditTeamController _auditTeamController;

        [Test]
        public void AddValidRequestTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Auditor>>()
                    .Setup(auditorsRepository => auditorsRepository.GetList())
                    .Returns(GetSampleAuditors());

                var identity = new GenericIdentity("test_user");
                identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()));
                var principal = new GenericPrincipal(identity, new[] { "user" });

                var httpCtxStub = new Mock<HttpContextBase>();
                httpCtxStub.SetupGet(p => p.User).Returns(principal);
                var controllerCtx = new ControllerContext
                {
                    HttpContext = httpCtxStub.Object

                };

                _auditTeamController = mock.Create<AuditTeamController>();
                _auditTeamController.ControllerContext = controllerCtx;

                var result = _auditTeamController.Add(Guid.NewGuid());

                var routeResult = result as RedirectToRouteResult;
                Assert.True((string)routeResult?.RouteValues["action"] == "Index");
            }
        }

        private List<Auditor> GetSampleAuditors()
        {
            return new List<Auditor>()
            {
                new Auditor()
                {
                    AuditorId = Guid.NewGuid()
                },
                new Auditor()
                {
                    AuditorId = Guid.NewGuid()
                },
            };
        }

        [Test]
        public void AddNullRequestTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                _auditTeamController = mock.Create<AuditTeamController>();

                var result = _auditTeamController.Add(null);

                var resultStatusCode = result as HttpStatusCodeResult;
                Assert.AreEqual(400,resultStatusCode?.StatusCode);
            }
        }

        [Test]
        public void AddEmptyGuidRequestTest()
        {
            using (var mock = AutoMock.GetLoose())
            {

                _auditTeamController = mock.Create<AuditTeamController>();

                var result = _auditTeamController.Add(Guid.Empty);

                var resultStatusCode = result as HttpStatusCodeResult;
                Assert.AreEqual(400, resultStatusCode?.StatusCode);
            }
        }
    }
}
