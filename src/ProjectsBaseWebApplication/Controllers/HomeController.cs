using System.Web.Mvc;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;

namespace ProjectsBaseWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;
        private readonly BaseRepository<Project> _projectsRepository;
        private bool _disposed;

        public HomeController(BaseRepository<Project> projectsRepository)
        {
            _context = new Context();
            _projectsRepository = projectsRepository;
        }
        public ActionResult Index()
        {
            var projects = _projectsRepository.GetList();

            return View(projects);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;

            base.Dispose(disposing);
        }
    }
}