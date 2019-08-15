using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectsBaseShared.Models
{
    public class Project : IEquatable<Project>
    {
        public Guid ProjectId { get; set; }
        [DisplayName("Client")] public Guid ClientId { get; set; }
        [Required, StringLength(100), DisplayName("Name")] public string ProjectName { get; set; }
        [DisplayName("Start date")] public DateTime ProjectStartDate { get; set; }
        [DisplayName("End date")] public DateTime ProjectEndDate { get; set; }
        public Client Client { get; set; }
        public ICollection<AuditTeam> Auditors { get; set; }

        public Project()
        {
            Auditors = new List<AuditTeam>();
        }

        public bool Equals(Project other)
        {
            return this.ProjectId == other?.ProjectId;
        }
    }
}
