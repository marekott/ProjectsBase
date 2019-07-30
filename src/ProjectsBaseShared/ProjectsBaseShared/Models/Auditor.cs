using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectsBaseShared.Models
{
    public class Auditor
    {
        public Guid AuditorId { get; set; }
        [Required, DisplayName("Name")]
        public string AuditorName { get; set; }
        [Required, DisplayName("Surname")]
        public string AuditorSurname { get; set; }
    }
}
