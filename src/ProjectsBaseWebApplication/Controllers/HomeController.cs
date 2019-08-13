using System;
using System.Linq;
using System.Web.Mvc;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;

namespace ProjectsBaseWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Project> _projectsRepository;

        public HomeController(IRepository<Project> projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }
        
        public ActionResult Index()
        {
            var projects = _projectsRepository.GetList();

            return View(projects);
        }
        [HttpPost]
        public ActionResult Index(string projectName)
        {
            var projects = _projectsRepository.GetList()
                .Where(p => p.ProjectName.ToLower()
                    .Contains(projectName.ToLower()))
                .ToList();

            return View(projects);
        }
        public ActionResult ProjectDetails(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var selectedOffer = _projectsRepository.Get((Guid)id);

            return View(selectedOffer);
        }
    }
}