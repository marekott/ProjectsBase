using System;
using NUnit.Framework;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Models
{
    [TestFixture]
    public class ProjectTests
    {
        private readonly Project _baseProject;

        public ProjectTests()
        {
            _baseProject = new Project()
            {
                ProjectId = Guid.NewGuid()
            };
        }

        [Test]
        public void TwoProjectsAreEqualTest()
        {
            var project = new Project()
            {
                ProjectId = _baseProject.ProjectId
            };

            Assert.True(_baseProject.Equals(project));
        }

        [Test]
        public void TwoProjectsAreNotEqualTest()
        {
            var project = new Project()
            {
                ProjectId = Guid.NewGuid()
            };

            Assert.False(_baseProject.Equals(project));
        }
    }
}
