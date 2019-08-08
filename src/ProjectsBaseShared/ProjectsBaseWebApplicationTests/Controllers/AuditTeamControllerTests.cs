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
                mock.Mock<BaseRepository<AuditTeam>>()
                    .Setup(auditTeamRepository => auditTeamRepository.Get(Guid.Empty, true))
                    .Returns(GetSampleAuditTeam());

                _auditTeamController = mock.Create<AuditTeamController>();

                var result = _auditTeamController.Add(Guid.NewGuid());

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public void AddNullRequestTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<BaseRepository<AuditTeam>>()
                    .Setup(auditTeamRepository => auditTeamRepository.Get(Guid.Empty, true))
                    .Returns(GetSampleAuditTeam());

                _auditTeamController = mock.Create<AuditTeamController>();

                var result = _auditTeamController.Add(null);

                var badRequestStatusCode = new HttpStatusCodeResult(400,null);
                Assert.IsInstanceOf<HttpStatusCodeResult>(result);
                Assert.AreEqual(badRequestStatusCode, result as HttpStatusCodeResult);
            }
        }

        [Test]
        public void AddEmptyGuidRequestTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<BaseRepository<AuditTeam>>()
                    .Setup(auditTeamRepository => auditTeamRepository.Get(Guid.Empty, true))
                    .Returns(GetSampleAuditTeam());

                _auditTeamController = mock.Create<AuditTeamController>();

                var result = _auditTeamController.Add(Guid.Empty);

                var badRequestStatusCode = new HttpStatusCodeResult(400, null);
                Assert.IsInstanceOf<HttpStatusCodeResult>(result);
                Assert.AreEqual(badRequestStatusCode, result as HttpStatusCodeResult);
            }
        }

        private AuditTeam GetSampleAuditTeam()
        {
            return new AuditTeam()
            {
                Id = Guid.Empty
            };
        }
    }
}
