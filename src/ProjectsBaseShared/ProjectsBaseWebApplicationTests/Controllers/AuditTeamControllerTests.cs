using System;
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
    public class AuditTeamControllerTests
    {
        private AuditTeamController _auditTeamController;

        [Test]
        public void AddValidRequestTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<AuditTeam>>()
                    .Setup(auditTeamRepository => auditTeamRepository.Add(new AuditTeam()));
                _auditTeamController = mock.Create<AuditTeamController>();

                var result = _auditTeamController.Add(Guid.NewGuid());

                RedirectToRouteResult routeResult = result as RedirectToRouteResult;
                Assert.True((string)routeResult?.RouteValues["action"] == "Index");
            }
        }

        [Test]
        public void AddNullRequestTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<AuditTeam>>()
                    .Setup(auditTeamRepository => auditTeamRepository.Add(new AuditTeam()));
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
                mock.Mock<IRepository<AuditTeam>>()
                    .Setup(auditTeamRepository => auditTeamRepository.Add(new AuditTeam()));
                _auditTeamController = mock.Create<AuditTeamController>();

                var result = _auditTeamController.Add(Guid.Empty);
                var resultStatusCode = result as HttpStatusCodeResult;

                Assert.AreEqual(400, resultStatusCode?.StatusCode);
            }
        }
    }
}
