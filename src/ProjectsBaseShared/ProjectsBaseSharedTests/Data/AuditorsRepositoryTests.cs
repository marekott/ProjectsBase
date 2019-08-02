using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using ProjectsBaseShared.Data;

namespace ProjectsBaseSharedTests.Data
{
    [TestFixture]
    public class AuditorsRepositoryTests
    {
        private DataMock _dataMock;

        [Test]
        public void AuditorsRepositoryCrudTests()
        {
            _dataMock = new DataMock();
            AddTest();
            GetOnlyAuditorTest();
            GetAuditorAndRelatedTest();
            GetAuditorsAndRelated();
            UpdateTest();
            DeleteTest();
        }

        private void AddTest()
        {
            using (var context = new Context())
            {
                var auditorsRepository = new AuditorsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                auditorsRepository.Add(_dataMock.Auditor);

                Assert.AreNotEqual(Guid.Empty, _dataMock.AuditorId, "Empty guid was return");
            }
        }

        private void GetOnlyAuditorTest()
        {
            using (var context = new Context())
            {
                var auditorsRepository = new AuditorsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedAuditor = auditorsRepository.Get(_dataMock.AuditorId, false);

                Assert.True(downloadedAuditor.Equals(_dataMock.Auditor), "GetOnlyAuditorTest returns auditor with different guid");
                Assert.AreEqual(_dataMock.AuditorName, downloadedAuditor.AuditorName, "GetOnlyAuditorTest returns auditor with different name");
                Assert.AreEqual(_dataMock.AuditorSurname, downloadedAuditor.AuditorSurname, "GetOnlyAuditorTest returns auditor with different surname");
                Assert.AreEqual(0, downloadedAuditor.Projects.Count, "GetOnlyAuditorTest returns related projects");
            }
        }

        private void GetAuditorAndRelatedTest()
        {
            using (var context = new Context())
            {
                var auditorsRepository = new AuditorsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedAuditor = auditorsRepository.Get(_dataMock.AuditorId);

                Assert.True(downloadedAuditor.Equals(_dataMock.Auditor), "GetAuditorAndRelatedTest returns auditor with different guid");
                Assert.AreEqual(_dataMock.AuditorName, downloadedAuditor.AuditorName, "GetAuditorAndRelatedTest returns auditor with different name");
                Assert.AreEqual(_dataMock.AuditorSurname, downloadedAuditor.AuditorSurname, "GetAuditorAndRelatedTest returns auditor with different surname");
                Assert.True(downloadedAuditor.Projects.Count > 0, "GetAuditorAndRelatedTest does not return related projects");
                Assert.True(downloadedAuditor.Projects.All(p => p.Project.Client != null), "GetAuditorAndRelatedTest does not return related clients");
            }
        }

        private void GetAuditorsAndRelated()
        {
            AddTest();

            using (var context = new Context())
            {
                var auditorsRepository = new AuditorsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var auditors = auditorsRepository.GetList();

                Assert.True(auditors.Count > 1, "GetAuditorsAndRelated returned only one auditor.");
                Assert.True(auditors.All(a => a.Projects.Count > 0), "GetAuditorsAndRelated does not return related projects");
                Assert.True(auditors.All(a => a.Projects.All(p => p.Project.Client != null)), "GetAuditorsAndRelated does not return related clients");
            }
        }

        private void UpdateTest()
        {
            const string newAuditorName = "New name";
            const string newAuditorSurname = "New name";

            using (var context = new Context())
            {
                var auditorsRepository = new AuditorsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                _dataMock.Auditor.AuditorName = newAuditorName;
                _dataMock.Auditor.AuditorSurname = newAuditorSurname;

                auditorsRepository.Update(_dataMock.Auditor);
            }

            using (var context = new Context())
            {
                var auditorsRepository = new AuditorsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedAuditor = auditorsRepository.Get(_dataMock.AuditorId);

                Assert.AreEqual(newAuditorName, downloadedAuditor.AuditorName);
                Assert.AreEqual(newAuditorSurname, downloadedAuditor.AuditorSurname);
                Assert.True(downloadedAuditor.Projects.Count > 0, "UpdateTest does not return related projects");
                Assert.True(downloadedAuditor.Projects.All(p => p.Project.Client != null), "UpdateTest does not return related clients");
            }
        }

        private void DeleteTest()
        {
            using (var context = new Context())
            {
                var auditorsRepository = new AuditorsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedAuditor = auditorsRepository.Get(_dataMock.AuditorId, false);
                var id = downloadedAuditor.AuditorId;
                Assert.IsNotNull(downloadedAuditor, "Auditor does not exist before delete.");

                auditorsRepository.Delete(downloadedAuditor);

                downloadedAuditor = auditorsRepository.Get(id, false);
                Assert.IsNull(downloadedAuditor, "Auditor exists after delete.");
            }
        }
    }
}
