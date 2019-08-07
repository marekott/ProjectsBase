using System;
using System.Linq;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Mock
{
    internal class ClientDataMock
    {
        public Guid ClientId => Client.ClientId;
        public Client Client { get; set; }
        public readonly string ClientName = "Test client";

        public ClientDataMock()
        {
            Client = new Client()
            {
                ClientName = ClientName
            };
            Client.Projects.Add(new ProjectDataMock(this).Project);
            Client.Projects.First().Auditors.Add(new AuditTeam() { Auditor = new AuditorDataMock(this).Auditor });
        }

        public ClientDataMock(Project projectDataMock)
        {
            Client = new Client()
            {
                ClientName = ClientName
            };
            Client.Projects.Add(projectDataMock);
            Client.Projects.First().Auditors.Add(new AuditTeam() { Auditor = new AuditorDataMock(projectDataMock).Auditor });
        }
    }
}
