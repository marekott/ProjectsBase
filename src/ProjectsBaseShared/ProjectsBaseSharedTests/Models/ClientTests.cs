using System;
using NUnit.Framework;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Models
{
    [TestFixture]
    public class ClientTests
    {
        private Client _baseClient;

        [SetUp]
        public void Init()
        {
            _baseClient = new Client()
            {
                ClientId = Guid.NewGuid()
            };
        }

        [Test]
        public void TwoProjectsAreEqualTest()
        {
            var client = new Client()
            {
                ClientId = _baseClient.ClientId
            };

            Assert.True(_baseClient.Equals(client));
        }

        [Test]
        public void TwoProjectsAreNotEqualTest()
        {
            var client = new Client()
            {
                ClientId = Guid.NewGuid()
            };

            Assert.False(_baseClient.Equals(client));
        }
    }
}
