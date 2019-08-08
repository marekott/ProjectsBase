using System;

namespace ProjectsBaseShared.Models
{
    public class AuditTeam
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid AuditorId { get; set; }
        public Project Project { get; set; }
        public Auditor Auditor { get; set; }
    }
}
