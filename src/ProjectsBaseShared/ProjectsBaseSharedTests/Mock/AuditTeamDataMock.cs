using System;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Mock
{
    internal class AuditTeamDataMock
    {
        public Guid AuditTeamId => AuditTeam.Id;
        public Guid ProjectId => AuditTeam.ProjectId;
        public Guid AuditorId => AuditTeam.AuditorId;
        public AuditTeam AuditTeam { get; set; }

        public AuditTeamDataMock(Guid projectId, Guid auditorId)
        {
            AuditTeam = new AuditTeam()
            {
                ProjectId = projectId,
                AuditorId = auditorId
            };
        }
    }
}
