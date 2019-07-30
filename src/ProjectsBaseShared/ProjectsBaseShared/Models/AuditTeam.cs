using System;

namespace ProjectsBaseShared.Models
{
    public class AuditTeam
    {
        public int Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid AuditorId { get; set; }
    }
}
