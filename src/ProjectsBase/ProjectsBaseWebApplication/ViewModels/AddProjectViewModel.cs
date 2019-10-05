using System.Collections.Generic;
using System.Web.Mvc;
using ProjectsBaseShared.Models;

namespace ProjectsBaseWebApplication.ViewModels
{
    public class AddProjectViewModel
    {
        public Project Project { get; set; } = new Project();
        public SelectList ClientsSelectList { get; set; }
        public void Init(List<Client> clients)
        {
            ClientsSelectList = new SelectList(clients, "ClientId", "ClientName");
        }
    }
}