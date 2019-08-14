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
                _auditTeamController = mock.Create<AuditTeamController>();

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
