using System;
using NUnit.Framework;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;

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

        private Context _context;
        private ProjectsRepository _projectsRepository;

        [SetUp]
        public void Init()
        {
            _context = new Context();
            _projectsRepository = new ProjectsRepository(_context);

            _project = InitProject();

            _projectsRepository.Add(_project);

            Assert.True(_project.ProjectId != Guid.Empty, "Add method returned empty guid.");

            _projectId = _project.ProjectId;
        }

        private Project InitProject()
        {
            return new Project()
            {
                ProjectName = ProjectName,
                ProjectStartDate = _projectStartDate,
                ProjectEndDate = _projectEndDate
            };
        }

        //[TearDown]
        //public void Remove()
        //{
        //    _projectsRepository.Delete(_project);
        //}

        [Test]
        public void GetOnlyProjectTest()
        {
            var downloadedProject = _projectsRepository.Get(_projectId, false);

            Assert.True(downloadedProject.Equals(_project), "GetOnlyProjectTest returned wrong id");
            Assert.AreEqual(ProjectName, downloadedProject.ProjectName, "GetOnlyProjectTest returned wrong project name.");
            Assert.AreEqual(_projectStartDate, downloadedProject.ProjectStartDate , "GetOnlyProjectTest returned wrong start date.");
            Assert.AreEqual(_projectEndDate, downloadedProject.ProjectEndDate, "GetOnlyProjectTest returned wrong end date.");
            Assert.True(downloadedProject.Auditors.Count == 0, "GetOnlyProjectTest returned related auditors.");
            Assert.IsNull(downloadedProject.Client, "GetOnlyProjectTest returned related client.");
        }

        [Test]
        public void GetOnlyProjectWhichNotExistTest()
        {
            var downloadedProject = _projectsRepository.Get(Guid.Empty, false);

            Assert.IsNull(downloadedProject);
        }

        //[Test]
        //public void GetProjectAndRelatedTest()
        //{
        //    //TODO
        //    throw new NotImplementedException();
        //}

        //[Test]
        //public void GetProjectAndRelatedWhichNotExistTest()
        //{
        //    //TODO
        //    throw new NotImplementedException();
        //}

        [Test]
        public void DeleteTest()
        {
            var downloadedProject = _projectsRepository.Get(_projectId, false);
            Assert.IsNotNull(downloadedProject, "Project is null before delete.");

            _projectsRepository.Delete(_project);
            downloadedProject = _projectsRepository.Get(_projectId, false);
            Assert.IsNull(downloadedProject, "Project is not null after delete.");
        }
    }
}
