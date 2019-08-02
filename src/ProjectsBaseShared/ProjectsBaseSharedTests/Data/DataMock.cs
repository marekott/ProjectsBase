using System;
using System.Linq;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Data
{
    internal class DataMock
    {
        public Guid ProjectId => Project.ProjectId;
        public Project Project { get; set; }
        public readonly string ProjectName = "Test project";
        public readonly DateTime ProjectStartDate = DateTime.Now;
        public readonly DateTime ProjectEndDate = DateTime.Now.AddDays(7);

        public Guid ClientId => Client.ClientId; 
        public Client Client { get; set; }
        public readonly string ClientName = "Test client";

        public readonly string AuditorName = "Marek";
        public readonly string AuditorSurname = "Ott";

        public DataMock()
        {
            Project = InitProject();
            Project.Client = InitClient();
            Project.Auditors.Add(InitAuditTeam());
            Project.Auditors.Add(InitAuditTeam());

            Client = InitClient();
            Client.Projects.Add(InitProject());
            Client.Projects.First().Auditors.Add(InitAuditTeam());
        }

        private Project InitProject()
        {
            return new Project()
            {
                ProjectName = ProjectName,
                ProjectStartDate = ProjectStartDate,
                ProjectEndDate = ProjectEndDate,
            };
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
                AuditorSurname = AuditorSurname
            };
        }
    }
}
