using System;
using System.Linq;
using System.Web.Mvc;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;
using ProjectsBaseWebApplication.Models;

namespace ProjectsBaseWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Project> _projectsRepository;
        private readonly IValidator<Project> _projectValidator;

        public HomeController(IRepository<Project> projectsRepository, IValidator<Project> projectValidator)
        {
            _projectsRepository = projectsRepository;
            _projectValidator = projectValidator;
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

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Project project)
        {
            if (ModelState.IsValid && _projectValidator.Validate(project))
            {
                _projectsRepository.Add(project);
                return RedirectToAction("Index");
            }

            return View(project);
        }
    }
}