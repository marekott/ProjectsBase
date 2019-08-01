using System;
using System.Diagnostics;
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

        private Context _context;
        private ProjectsRepository _projectsRepository;

        [Test]
        public void ProjectsRepositoryCrud()
        {
            _projectId = Create();
            GetOnlyProjectTest();
            GetOnlyProjectWhichNotExistTest();
            GetProjectAndRelatedTest();
            GetProjectAndRelatedWhichNotExistTest();
            //TODO update
            DeleteTest();
        }

        private Guid Create()
        {
            _context = new Context();
            _context.Database.Log = (message) => Debug.WriteLine(message);
            _projectsRepository = new ProjectsRepository(_context);
            _project = InitProject();

            _projectsRepository.Add(_project);

            Assert.AreNotEqual(Guid.Empty, _project.ProjectId, "Empty guid was return");

            return _project.ProjectId;
        }

        private Project InitProject()
        {
            var project = new Project()
            {
                ProjectName = ProjectName,
                ProjectStartDate = _projectStartDate,
                ProjectEndDate = _projectEndDate,
                Client = InitClient()
            };
            project.Auditors.Add(InitAuditTeam());
            project.Auditors.Add(InitAuditTeam());

            return project;
        }

        private Client InitClient()
        {
            return new Client()
            {
                ClientName = ClientName
            };
        }

        private AuditTeam InitAuditTeam()
        {
            return new AuditTeam()
            {
                Auditor = InitAuditor()
            };
        }

        private Auditor InitAuditor()
        {
            return new Auditor()
            {
                AuditorName = AuditorName,
                AuditorSurname = AuditorSurname + new Random().Next()
            };
        }


        public void GetOnlyProjectTest()
        {
            var downloadedProject = _projectsRepository.Get(_projectId, false);

            Assert.True(downloadedProject.Equals(_project), "GetOnlyProjectTest returns project with different guid");
            Assert.True(downloadedProject.ProjectName == ProjectName, "GetOnlyProjectTest returns project with different name");
            Assert.True(downloadedProject.ProjectStartDate == _projectStartDate, "GetOnlyProjectTest returns project with different start date");
            Assert.True(downloadedProject.ProjectEndDate == _projectEndDate, "GetOnlyProjectTest returns project with different end date");
            Assert.True(downloadedProject.Auditors.Count == 0, "GetOnlyProjectTest returns related auditors");
            Assert.IsNull(downloadedProject.Client, "GetOnlyProjectTest returns related client");
        }

        public void GetOnlyProjectWhichNotExistTest()
        {
            var downloadedProject = _projectsRepository.Get(Guid.Empty, false);

            Assert.IsNull(downloadedProject, "Database contains record with empty guid.");
        }

        public void GetProjectAndRelatedTest()
        {
            var downloadedProject = _projectsRepository.Get(_projectId);

            Assert.True(downloadedProject.Equals(_project), "GetProjectAndRelatedTest returns project with different guid");
            Assert.True(downloadedProject.ProjectName == ProjectName, "GetProjectAndRelatedTest returns project with different name");
            Assert.True(downloadedProject.ProjectStartDate == _projectStartDate, "GetProjectAndRelatedTest returns project with different start date");
            Assert.True(downloadedProject.ProjectEndDate == _projectEndDate, "GetProjectAndRelatedTest returns project with different end date");
            Assert.True(downloadedProject.Auditors.Count != 0, "GetProjectAndRelatedTest does not returns related auditors");
            Assert.IsNotNull(downloadedProject.Client, "GetProjectAndRelatedTest does not return related client");
        }

        public void GetProjectAndRelatedWhichNotExistTest()
        {
            var downloadedProject = _projectsRepository.Get(Guid.Empty);

            Assert.IsNull(downloadedProject, "Database contains record with empty guid.");
        }

        public void DeleteTest()
        {
            var downloadedProject = _projectsRepository.Get(_projectId, false);
            Assert.True(downloadedProject != null, "Project does not exist before delete.");

            _projectsRepository.Delete(_project);

            downloadedProject = _projectsRepository.Get(_projectId, false);
            Assert.True(downloadedProject == null, "Project exists after delete.");
        }
    }
}
