using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectsBaseShared.Models
{
    public class Auditor : IEquatable<Auditor>
    {
        public Guid AuditorId { get; set; }
        [Required, DisplayName("Name")] public string AuditorName { get; set; }
        [Required, DisplayName("Surname")] public string AuditorSurname { get; set; }
        public ICollection<AuditTeam> Projects { get; set; }

        public Auditor()
        {
            Projects = new List<AuditTeam>();
        }

        public bool Equals(Auditor other)
        {
            return this.AuditorId == other?.AuditorId;
        }
    }
}
