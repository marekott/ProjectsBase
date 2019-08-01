using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;
using Assert = NUnit.Framework.Assert;

namespace ProjectsBaseSharedTests.Data
{
    [TestFixture]
    public class ProjectsRepositoryTests
    {
        private Guid _projectId;
        private Project _project;
        private const string ProjectName = "Test project";
        private readonly DateTime _projectStartDate = DateTime.Now;
        private readonly DateTime _projectEndDate = DateTime.Now.AddDays(7);

        private const string ClientName = "Test client";

        private const string AuditorName = "Marek";
        private const string AuditorSurname = "Ott";


        [Test]
        public void ProjectsRepositoryCrud()
        {
            _projectId = Create(ProjectName, _projectStartDate, _projectEndDate, ClientName);
            GetOnlyProjectTest();
            GetOnlyProjectWhichNotExistTest();
            GetProjectAndRelatedTest();
            GetProjectAndRelatedWhichNotExistTest();
            //TODO update
            DeleteTest();
        }

        private Guid Create(string projectName, DateTime projectStartDate, DateTime projectEndDate, string clientName)
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);
                _project = ProjectDataMock.InitProject(projectName, projectStartDate, projectEndDate, clientName, AuditorName, AuditorSurname);

                projectsRepository.Add(_project);

                Assert.AreNotEqual(Guid.Empty, _project.ProjectId, "Empty guid was return");

                return _project.ProjectId;
            }
            
        }

        public void GetOnlyProjectTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);

                var downloadedProject = projectsRepository.Get(_projectId, false);

                Assert.True(downloadedProject.Equals(_project), "GetOnlyProjectTest returns project with different guid");
                Assert.AreEqual(ProjectName, downloadedProject.ProjectName, "GetOnlyProjectTest returns project with different name");
                Assert.AreEqual(_projectStartDate.Date, downloadedProject.ProjectStartDate.Date, "GetOnlyProjectTest returns project with different start date");
                Assert.AreEqual(_projectEndDate.Date, downloadedProject.ProjectEndDate.Date, "GetOnlyProjectTest returns project with different end date");
                Assert.AreEqual(0, downloadedProject.Auditors.Count, "GetOnlyProjectTest returns related auditors");
                Assert.IsNull(downloadedProject.Client, "GetOnlyProjectTest returns related client");
            }
            
        }

        public void GetOnlyProjectWhichNotExistTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);

                var downloadedProject = projectsRepository.Get(Guid.Empty, false);

                Assert.IsNull(downloadedProject, "Database contains record with empty guid.");
            }
        }

        public void GetProjectAndRelatedTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);

                var downloadedProject = projectsRepository.Get(_projectId);

                Assert.True(downloadedProject.Equals(_project), "GetProjectAndRelatedTest returns project with different guid");
                Assert.AreEqual(ProjectName, downloadedProject.ProjectName , "GetProjectAndRelatedTest returns project with different name");
                Assert.AreEqual(_projectStartDate.Date, downloadedProject.ProjectStartDate.Date , "GetProjectAndRelatedTest returns project with different start date");
                Assert.AreEqual(_projectEndDate.Date, downloadedProject.ProjectEndDate.Date, "GetProjectAndRelatedTest returns project with different end date");
                Assert.AreEqual(2, downloadedProject.Auditors.Count, "GetProjectAndRelatedTest does not returns related auditors");
                Assert.IsNotNull(downloadedProject.Auditors.ToList()[0].Auditor);
                Assert.AreEqual(AuditorName, downloadedProject.Auditors.ToList()[0].Auditor.AuditorName);
                Assert.AreEqual(AuditorSurname, downloadedProject.Auditors.ToList()[0].Auditor.AuditorSurname);
                Assert.IsNotNull(downloadedProject.Auditors.ToList()[1].Auditor);
                Assert.AreEqual(AuditorName, downloadedProject.Auditors.ToList()[1].Auditor.AuditorName);
                Assert.AreEqual(AuditorSurname, downloadedProject.Auditors.ToList()[1].Auditor.AuditorSurname);
                Assert.AreEqual(2, downloadedProject.Auditors.Select(a => a.Auditor.AuditorId).Distinct().Count());
                Assert.IsNotNull(downloadedProject.Client, "GetProjectAndRelatedTest does not return related client");
                Assert.AreEqual(ClientName, downloadedProject.Client.ClientName, "GetProjectAndRelatedTest returned wrong client name");
            }
        }

        public void GetProjectAndRelatedWhichNotExistTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);

                var downloadedProject = projectsRepository.Get(Guid.Empty);

                Assert.IsNull(downloadedProject, "Database contains record with empty guid.");
            }
        }

        public void DeleteTest()
        {
            using (var context = new Context())
            {
                var projectsRepository = new ProjectsRepository(context);

                var downloadedProject = projectsRepository.Get(_projectId, false);
                Assert.True(downloadedProject != null, "Project does not exist before delete.");

                projectsRepository.Delete(downloadedProject);

                downloadedProject = projectsRepository.Get(_projectId, false);
                Assert.True(downloadedProject == null, "Project exists after delete.");
            }
        }
    }
}
