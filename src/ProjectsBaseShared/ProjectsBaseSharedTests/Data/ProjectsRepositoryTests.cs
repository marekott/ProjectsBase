using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using ProjectsBaseShared.Data;
using ProjectsBaseSharedTests.Mock;

namespace ProjectsBaseSharedTests.Data
{
    [TestFixture]
    public class ProjectsRepositoryTests //TODO dodać implementacje Dispose to moze nie bd trzeba robic using w kazdej metodzie osobno
    {
        private ProjectDataMock _projectDataMock;

        [SetUp]
        public void CleanUp()
        {
            using (var context = new Context())
            {
                context.Database.Delete();
            }
        }

        [Test]
        public void ProjectsRepositoryCrudTests()
        {
            _projectDataMock = new ProjectDataMock();
            AddTest();
            GetOnlyProjectTest();
            GetProjectAndRelatedTest();
            GetProjectsAndRelated();
            UpdateTest();
            DeleteTest();
        }

        private void AddTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                projectsRepository.Add(_projectDataMock.Project);

                Assert.AreNotEqual(Guid.Empty, _projectDataMock.ProjectId, "Empty guid was return");
            }
        }

        private void GetOnlyProjectTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedProject = projectsRepository.Get(_projectDataMock.ProjectId, false);

                Assert.True(downloadedProject.Equals(_projectDataMock.Project), "GetOnlyProjectTest returns project with different guid");
                Assert.AreEqual(_projectDataMock.ProjectName, downloadedProject.ProjectName, "GetOnlyProjectTest returns project with different name");
                Assert.AreEqual(_projectDataMock.ProjectStartDate.Date, downloadedProject.ProjectStartDate.Date, "GetOnlyProjectTest returns project with different start date");
                Assert.AreEqual(_projectDataMock.ProjectEndDate.Date, downloadedProject.ProjectEndDate.Date, "GetOnlyProjectTest returns project with different end date");
                Assert.AreEqual(0, downloadedProject.Auditors.Count, "GetOnlyProjectTest returns related auditors");
                Assert.IsNull(downloadedProject.Client, "GetOnlyProjectTest returns related client");
            }
        }

        private void GetProjectAndRelatedTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedProject = projectsRepository.Get(_projectDataMock.ProjectId);

                Assert.True(downloadedProject.Equals(_projectDataMock.Project), "GetProjectAndRelatedTest returns project with different guid");
                Assert.AreEqual(_projectDataMock.ProjectName, downloadedProject.ProjectName , "GetProjectAndRelatedTest returns project with different name");
                Assert.AreEqual(_projectDataMock.ProjectStartDate.Date, downloadedProject.ProjectStartDate.Date , "GetProjectAndRelatedTest returns project with different start date");
                Assert.AreEqual(_projectDataMock.ProjectEndDate.Date, downloadedProject.ProjectEndDate.Date, "GetProjectAndRelatedTest returns project with different end date");
                Assert.AreEqual(2, downloadedProject.Auditors.Count, "GetProjectAndRelatedTest does not returns related auditors");
                Assert.IsNotNull(downloadedProject.Auditors.ToList()[0].Auditor);
                Assert.AreEqual(_projectDataMock.Project.Auditors.First().Auditor.AuditorName, downloadedProject.Auditors.ToList()[0].Auditor.AuditorName);
                Assert.AreEqual(_projectDataMock.Project.Auditors.First().Auditor.AuditorSurname, downloadedProject.Auditors.ToList()[0].Auditor.AuditorSurname);
                Assert.IsNotNull(downloadedProject.Auditors.ToList()[1].Auditor);
                Assert.AreEqual(_projectDataMock.Project.Auditors.Skip(1).First().Auditor.AuditorName, downloadedProject.Auditors.ToList()[1].Auditor.AuditorName);
                Assert.AreEqual(_projectDataMock.Project.Auditors.Skip(1).First().Auditor.AuditorSurname, downloadedProject.Auditors.ToList()[1].Auditor.AuditorSurname);
                Assert.AreEqual(2, downloadedProject.Auditors.Select(a => a.Auditor.AuditorId).Distinct().Count());
                Assert.IsNotNull(downloadedProject.Client, "GetProjectAndRelatedTest does not return related client");
                Assert.AreEqual(_projectDataMock.Project.Client.ClientName, downloadedProject.Client.ClientName, "GetProjectAndRelatedTest returned wrong client name");
            }
        }

        private void GetProjectsAndRelated()
        {
            AddTest();

            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var projects = projectsRepository.GetList();

                Assert.True(projects.Count > 1, "GetProjectsAndRelated returned only one project.");
                Assert.True(projects.All(p => p.Client != null), "GetProjectsAndRelated does not return related clients.");
                Assert.True(projects.All(p => p.Auditors.Count != 0), "GetProjectsAndRelated does not return related auditors.");
            }
        }

        private void UpdateTest()
        {
            string newProjectName = "New name";
            DateTime newStartDate = DateTime.Now.AddYears(1);
            DateTime newEndDate = DateTime.Now.AddYears(2);

            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                _projectDataMock.Project.ProjectName = newProjectName;
                _projectDataMock.Project.ProjectStartDate = newStartDate;
                _projectDataMock.Project.ProjectEndDate = newEndDate;

                projectsRepository.Update(_projectDataMock.Project);
            }

            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedProject = projectsRepository.Get(_projectDataMock.ProjectId);

                Assert.AreEqual(newProjectName, downloadedProject.ProjectName);
                Assert.AreEqual(newStartDate.Date, downloadedProject.ProjectStartDate.Date);
                Assert.AreEqual(newEndDate.Date, downloadedProject.ProjectEndDate.Date);
                Assert.IsNotNull(downloadedProject.Client);
                Assert.True(downloadedProject.Auditors.Count != 0);
            }
        }

        private void DeleteTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedProject = projectsRepository.Get(_projectDataMock.ProjectId, false);
                var id = downloadedProject.ProjectId;
                Assert.IsNotNull(downloadedProject, "Project does not exist before delete.");

                projectsRepository.Delete(downloadedProject);

                downloadedProject = projectsRepository.Get(id, false);
                Assert.IsNull(downloadedProject, "Project exists after delete.");
            }
        }
    }
}
