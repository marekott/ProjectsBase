using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using ProjectsBaseShared.Data;
using ProjectsBaseSharedTests.Mock;

namespace ProjectsBaseSharedTests.Data
{
    [TestFixture]
    public class AuditTeamRepositoryTest
    {
        private AuditTeamDataMock _auditTeamDataMock;
        
        [SetUp]
        public void CleanUp()
        {
            using (var context = new Context())
            {   
                context.Database.Delete();
            }
        }

        [Test]
        public void AuditTeamRepositoryCrudTests()
        {
            InitData();
            AddTest();
            GetOnlyAuditTeamTest();
            GetAuditTeamAndRelatedTest();
            GetAuditTeamsAndRelatedTest();
            UpdateTest();
            DeleteTest();
        }

        private void InitData()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var project = new ProjectDataMock();

                projectsRepository.Add(project.Project);

                _auditTeamDataMock = new AuditTeamDataMock(project.ProjectId, project.Project.Auditors.First().AuditorId);
            }
        }

        private void AddTest()
        {
            using (var context = new Context())
            {
                var auditTeamRepository = new AuditTeamRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                auditTeamRepository.Add(_auditTeamDataMock.AuditTeam);

                Assert.AreNotEqual(Guid.Empty, _auditTeamDataMock.AuditTeamId, "Empty guid was return");
            }
        }

        private void GetOnlyAuditTeamTest()
        {
            using (var context = new Context())
            {
                var auditTeamRepository = new AuditTeamRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedAuditTeam = auditTeamRepository.Get(_auditTeamDataMock.AuditTeamId, false);

                Assert.AreEqual(_auditTeamDataMock.AuditTeamId, downloadedAuditTeam.Id, "GetOnlyAuditTeamTest returns audit team with different guid");
                Assert.IsNull(downloadedAuditTeam.Project, "GetOnlyAuditTeamTest returns related projects");
                Assert.IsNull(downloadedAuditTeam.Auditor, "GetOnlyAuditTeamTest returns related auditors");
            }
        }

        private void GetAuditTeamAndRelatedTest()
        {
            using (var context = new Context())
            {
                var auditTeamRepository = new AuditTeamRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedAuditTeam = auditTeamRepository.Get(_auditTeamDataMock.AuditTeamId);

                Assert.AreEqual(_auditTeamDataMock.AuditTeamId, downloadedAuditTeam.Id, "GetAuditTeamAndRelatedTest returns audit team with different guid");
                Assert.IsNotNull(downloadedAuditTeam.Project, "GetAuditTeamAndRelatedTest does not returns related projects");
                Assert.IsNotNull(downloadedAuditTeam.Auditor, "GetAuditTeamAndRelatedTest does not returns related auditors");
            }
        }

        private void GetAuditTeamsAndRelatedTest()
        {
            using (var context = new Context())
            {
                var auditTeamRepository = new AuditTeamRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                Assert.Throws<NotImplementedException>( () => auditTeamRepository.GetList());
            }
        }

        private void UpdateTest()
        {
            var newProject = new ProjectDataMock();

            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                projectsRepository.Add(newProject.Project);

                _auditTeamDataMock.AuditTeam.ProjectId = newProject.ProjectId;
                _auditTeamDataMock.AuditTeam.AuditorId = newProject.Project.Auditors.First().AuditorId;
            }

            using (var context = new Context())
            {
                var auditTeamRepository = new AuditTeamRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);
                auditTeamRepository.Update(_auditTeamDataMock.AuditTeam);
            }

            using (var context = new Context())
            {
                var auditTeamRepository = new AuditTeamRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedAuditTeam = auditTeamRepository.Get(_auditTeamDataMock.AuditTeamId);

                Assert.AreEqual(newProject.ProjectId, downloadedAuditTeam.ProjectId);
                Assert.AreEqual(newProject.Project.Auditors.First().AuditorId, downloadedAuditTeam.AuditorId);
            }
        }

        private void DeleteTest()
        {
            using (var context = new Context())
            {
                var auditTeamRepository = new AuditTeamRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedAuditTeam = auditTeamRepository.Get(_auditTeamDataMock.AuditTeamId);
                var id = downloadedAuditTeam.Id;
                Assert.IsNotNull(downloadedAuditTeam, "AuditTeam does not exist before delete.");

                auditTeamRepository.Delete(downloadedAuditTeam);

                downloadedAuditTeam = auditTeamRepository.Get(id);
                Assert.IsNull(downloadedAuditTeam, "AuditTeam exists after delete.");
            }
        }
    }
}
