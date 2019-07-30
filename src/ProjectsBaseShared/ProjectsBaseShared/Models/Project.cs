﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectsBaseShared.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        [Required, StringLength(100), DisplayName("Name")]
        public string ProjectName { get; set; }
        [DisplayName("Start date")]
        public DateTime ProjectStartDate { get; set; }
        [DisplayName("End date")]
        public DateTime ProjectEndDate { get; set; }
    }
}
