using System;
using System.Linq;
using System.Web.Mvc;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;

namespace ProjectsBaseWebApplication.Controllers
{
    public class HomeController : Controller
    {
        //private readonly Context _context; //TODO do usunięcia jak ustali się gdzie ma być dispose
        private readonly BaseRepository<Project> _projectsRepository;
        //private bool _disposed; //TODO do usunięcia jak ustali się gdzie ma być dispose

        public HomeController(BaseRepository<Project> projectsRepository)
        {
            //_context = new Context(); //TODO do usunięcia jak ustali się gdzie ma być dispose
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

        //protected override void Dispose(bool disposing) //TODO ustalić gdzie powinno być dispose robione na context bo na pewno nie tu
        //{
        //    if (_disposed)
        //    {
        //        return;
        //    }

        //    if (disposing)
        //    {
        //        _context.Dispose();
        //    }

        //    _disposed = true;

        //    base.Dispose(disposing);
        //}
    }
}