using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using ProjectsBaseShared.Data;

namespace ProjectsBaseSharedTests.Data
{
    public class ClientsRepositoryTests
    {
        private DataMock _dataMock;

        [Test]
        public void ClientsRepositoryCrud()
        {
            _dataMock = new DataMock();
            AddTest();
            GetOnlyClientTest();
            GetClientAndRelatedTest();
            GetClientsAndRelated();
            UpdateTest();
            DeleteTest();
        }

        private void AddTest()
        {
            using (var context = new Context())
            {
                var clientsRepository = new ClientsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                clientsRepository.Add(_dataMock.Client);

                Assert.AreNotEqual(Guid.Empty, _dataMock.ClientId, "Empty guid was return");
            }
        }

        private void GetOnlyClientTest()
        {
            using (var context = new Context())
            {
                var clientsRepository = new ClientsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedClient = clientsRepository.Get(_dataMock.ClientId, false);

                Assert.True(downloadedClient.Equals(_dataMock.Client), "GetOnlyClientTest returns client with different guid");
                Assert.AreEqual(_dataMock.ClientName, downloadedClient.ClientName, "GetOnlyClientTest returns client with different name");
                Assert.AreEqual(0, downloadedClient.Projects.Count, "GetOnlyClientTest returns related projects");
            }
        }

        private void GetClientAndRelatedTest()
        {
            using (var context = new Context())
            {
                var clientsRepository = new ClientsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedClient = clientsRepository.Get(_dataMock.ClientId, false);

                Assert.True(downloadedClient.Equals(_dataMock.Client), "GetClientAndRelatedTest returns client with different guid");
                Assert.AreEqual(_dataMock.ClientName, downloadedClient.ClientName, "GetClientAndRelatedTest returns client with different name");
                Assert.True(downloadedClient.Projects.Count > 0, "GetClientAndRelatedTest does not return related projects");
                Assert.True(downloadedClient.Projects.All(p => p.Auditors.Count > 0), "GetClientAndRelatedTest does not return related auditors");
            }
        }

        private void GetClientsAndRelated()
        {
            AddTest();

            using (var context = new Context())
            {
                var clientsRepository = new ClientsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var clients = clientsRepository.GetList();

                Assert.True(clients.Count > 1, "GetClientsAndRelated returned only one client.");
                Assert.True(clients.All(c => c.Projects.Count > 0), "GetClientsAndRelated does not return related projects");
                Assert.True(clients.All(c => c.Projects.All(p => p.Auditors.Count > 0)), "GetClientsAndRelated does not return related auditors");
            }
        }

        private void UpdateTest()
        {
            string newClientName = "New name";

            using (var context = new Context())
            {
                var clientsRepository = new ClientsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                _dataMock.Client.ClientName = newClientName;

                clientsRepository.Update(_dataMock.Client);
            }

            using (var context = new Context())
            {
                var clientsRepository = new ClientsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedClient = clientsRepository.Get(_dataMock.ClientId, false);

                Assert.AreEqual(newClientName, downloadedClient.ClientName);
                Assert.True(downloadedClient.Projects.Count > 0, "UpdateTest does not return related projects");
                Assert.True(downloadedClient.Projects.All(p => p.Auditors.Count > 0), "UpdateTest does not return related auditors");
            }
        }

        private void DeleteTest()
        {
            using (var context = new Context())
            {
                var clientsRepository = new ClientsRepository(context);
                context.Database.Log = (message) => Debug.WriteLine(message);

                var downloadedClient = clientsRepository.Get(_dataMock.ClientId, false);
                var id = downloadedClient.ClientId;
                Assert.IsNotNull(downloadedClient, "Client does not exist before delete.");

                clientsRepository.Delete(downloadedClient);

                downloadedClient = clientsRepository.Get(id, false);
                Assert.IsNull(downloadedClient, "Client exists after delete.");
            }
        }
    }
}
