using System;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Data
{
    internal class ProjectDataMock
    {
        public static Project InitProject(string projectName, DateTime projectStartDate, DateTime projectEndDate, string clientName, string auditorName, string auditorSurname)
        {
            var project = new Project()
            {
                ProjectName = projectName,
                ProjectStartDate = projectStartDate,
                ProjectEndDate = projectEndDate,
                Client = InitClient(clientName)
            };
            project.Auditors.Add(InitAuditTeam(auditorName, auditorSurname));
            project.Auditors.Add(InitAuditTeam(auditorName, auditorSurname));

            return project;
        }

        public static Client InitClient(string clientName)
        {
            return new Client()
            {
                ClientName = clientName
            };
        }

        public static AuditTeam InitAuditTeam(string auditorName, string auditorSurname)
        {
            return new AuditTeam()
            {
                Auditor = InitAuditor(auditorName, auditorSurname)
            };
        }

        public static Auditor InitAuditor(string auditorName, string auditorSurname)
        {
            return new Auditor()
            {
                AuditorName = auditorName,
                AuditorSurname = auditorSurname
            };
        }
    }
}
