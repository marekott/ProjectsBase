using System;
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
        }

        public ClientDataMock(Project projectDataMock)
        {
            Client = new Client()
            {
                ClientName = ClientName
            };
            Client.Projects.Add(projectDataMock);
        }
    }
}
