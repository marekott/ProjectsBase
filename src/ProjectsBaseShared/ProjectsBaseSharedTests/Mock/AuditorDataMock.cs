using System;
using System.Linq;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Mock
{
    internal class AuditorDataMock
    {
        public Guid AuditorId => Auditor.AuditorId;
        public Auditor Auditor { get; set; }
        public readonly string AuditorName = "Marek";
        public readonly string AuditorSurname = "Ott";

        public AuditorDataMock()
        {
            Auditor = new Auditor()
            {
                AuditorName = AuditorName,
                AuditorSurname = AuditorSurname
            };
            Auditor.Projects.Add(new AuditTeam(){Auditor = Auditor});
            Auditor.Projects.First().Project = new ProjectDataMock(this).Project;
            Auditor.Projects.First().Project.Client = new ClientDataMock(Auditor.Projects.First().Project).Client;
        }
    }
}
