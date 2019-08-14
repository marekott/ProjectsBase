using System;
using NUnit.Framework;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Models
{
    [TestFixture]
    public class AuditorTests
    {
        private Auditor _baseAuditor;

        [SetUp]
        public void Init()
        {
            _baseAuditor = new Auditor()
            {
                AuditorId = Guid.NewGuid()
            };
        }

        [Test]
        public void TwoAuditorsAreEqual()
        {
            var auditor = new Auditor()
            {
                AuditorId = _baseAuditor.AuditorId
            };

            Assert.True(_baseAuditor.Equals(auditor));
        }

        [Test]
        public void TwoAuditorsAreNotEqual()
        {
            var auditor = new Auditor()
            {
                AuditorId = Guid.NewGuid()
            };

            Assert.False(_baseAuditor.Equals(auditor));
        }
    }
}
