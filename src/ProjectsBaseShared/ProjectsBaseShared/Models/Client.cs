using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectsBaseShared.Models
{
    public class Client : IEquatable<Client>
    {
        public Guid ClientId { get; set; }
        [Required, DisplayName("Name")]
        public string ClientName { get; set; }

        public ICollection<Project> Projects { get; set; }

        public Client()
        {
            Projects = new List<Project>();
        }

        public bool Equals(Client other)
        {
            return this.ClientId == other?.ClientId;
        }
    }
}
