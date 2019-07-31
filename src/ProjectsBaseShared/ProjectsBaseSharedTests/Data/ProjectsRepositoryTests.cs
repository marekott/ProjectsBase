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

        [TearDown]
        public void Remove()
        {
            _projectsRepository.Delete(_project);
        }

        [Test]
        public void GetOnlyProjectTest()
        {
            var downloadedProject = _projectsRepository.Get(_projectId, false);

            Assert.True(downloadedProject.Equals(_project));
        }

        [Test]
        public void GetOnlyProjectWhichNotExistTest()
        {
            var downloadedProject = _projectsRepository.Get(Guid.Empty, false);

            Assert.IsNull(downloadedProject);
        }

        [Test]
        public void GetProjectAndRelatedTest()
        {
            //TODO
            throw new NotImplementedException();
        }

        [Test]
        public void GetProjectAndRelatedWhichNotExistTest()
        {
            //TODO
            throw new NotImplementedException();
        }

        [Test]
        public void AddTest() //add method is called during SetUp so if _project exists in the database the test has passed. 
        {
            GetOnlyProjectTest();
        }

        [Test]
        public void DeleteTest()
        {
            var project = InitProject();

            _projectsRepository.Add(project);
            var downloadedProject = _projectsRepository.Get(project.ProjectId, false);
            Assert.True(downloadedProject != null);

            _projectsRepository.Delete(project);
            downloadedProject = _projectsRepository.Get(project.ProjectId, false);
            Assert.True(downloadedProject == null);
        }
    }
}
