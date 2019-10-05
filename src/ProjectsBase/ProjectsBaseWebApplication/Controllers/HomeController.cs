using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;
using ProjectsBaseWebApplication.Models;
using ProjectsBaseWebApplication.ViewModels;

namespace ProjectsBaseWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Project> _projectsRepository;
        private readonly IRepository<Client> _clientsRepository;
        private readonly IValidator<Project> _projectValidator;

        public HomeController(IRepository<Project> projectsRepository, IRepository<Client> clientsRepository,
            IValidator<Project> projectValidator)
        {
            _projectsRepository = projectsRepository;
            _projectValidator = projectValidator;
            _clientsRepository = clientsRepository;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var projects = _projectsRepository.GetList();

            return View(projects);
        }

        [HttpPost, ValidateAntiForgeryToken]
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

            var selectedOffer = _projectsRepository.Get((Guid) id);

            return View(selectedOffer);
        }

        public ActionResult Add()
        {
            var viewModel = new AddProjectViewModel();
            viewModel.Project.UserId = User.Identity.GetUserId();
            viewModel.Init(_clientsRepository.GetList());
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(AddProjectViewModel viewModel) //TODO dodać walidację kliencką na polach z datą
        {
            viewModel.Project.UserId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                if (_projectValidator.Validate(viewModel.Project))
                {
                    _projectsRepository.Add(viewModel.Project);
                    TempData["Message"] = "Your project was successfully added!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("",
                    "Project end date should be the same or later than the start date. Both should be a future date.");
            }

            viewModel.Init(_clientsRepository.GetList());

            return View(viewModel);
        }

        public ActionResult MyProjects()
        {
            var userId = User.Identity.GetUserId();

            var myProjects = _projectsRepository.GetList()
                .Where(p => p.UserId == userId)
                .ToList();

            return View(myProjects);
        }
    }
}