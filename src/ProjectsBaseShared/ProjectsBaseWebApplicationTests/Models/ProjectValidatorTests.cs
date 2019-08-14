using System;
using NUnit.Framework;
using ProjectsBaseShared.Models;
using ProjectsBaseWebApplication.Models;
using Assert = NUnit.Framework.Assert;

namespace ProjectsBaseWebApplicationTests.Models
{
    [TestFixture]
    public class ProjectValidatorTests
    {
        private ProjectValidator _projectValidator;

        [Test]
        public void ValidatePositiveTest()
        {
            var project = new Project()
            {
                ProjectName = "Test",
                ProjectStartDate = DateTime.Now.AddDays(1),
                ProjectEndDate = DateTime.Now.AddDays(1),
            };
            _projectValidator = new ProjectValidator();

            var result = _projectValidator.Validate(project);

            Assert.True(result);
        }

        [Test]
        public void ValidateNegativeStartDateIsPastTest()
        {
            var project = new Project()
            {
                ProjectName = "Test",
                ProjectStartDate = DateTime.Now.AddDays(-1),
                ProjectEndDate = DateTime.Now.AddDays(1),
            };
            _projectValidator = new ProjectValidator();

            var result = _projectValidator.Validate(project);

            Assert.False(result);
        }

        [Test]
        public void ValidateNegativeStartDateIsTodayTest()
        {
            var project = new Project()
            {
                ProjectName = "Test",
                ProjectStartDate = DateTime.Now,
                ProjectEndDate = DateTime.Now.AddDays(1),
            };
            _projectValidator = new ProjectValidator();

            var result = _projectValidator.Validate(project);

            Assert.False(result);
        }

        [Test]
        public void ValidateNegativeEndDateIsPastTest()
        {
            var project = new Project()
            {
                ProjectName = "Test",
                ProjectStartDate = DateTime.Now.AddDays(1),
                ProjectEndDate = DateTime.Now.AddDays(-1),
            };
            _projectValidator = new ProjectValidator();

            var result = _projectValidator.Validate(project);

            Assert.False(result);
        }

        [Test]
        public void ValidateNegativeEndDateIsTodayTest()
        {
            var project = new Project()
            {
                ProjectName = "Test",
                ProjectStartDate = DateTime.Now.AddDays(1),
                ProjectEndDate = DateTime.Now,
            };
            _projectValidator = new ProjectValidator();

            var result = _projectValidator.Validate(project);

            Assert.False(result);
        }

        [Test]
        public void ValidateNegativeEndDateIsEarlierThanStartDateTest()
        {
            var project = new Project()
            {
                ProjectName = "Test",
                ProjectStartDate = DateTime.Now.AddDays(2),
                ProjectEndDate = DateTime.Now.AddDays(1),
            };
            _projectValidator = new ProjectValidator();

            var result = _projectValidator.Validate(project);

            Assert.False(result);
        }
    }
}
