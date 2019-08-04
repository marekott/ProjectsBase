using System.Linq;
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
        [HttpPost]
        public object Index(string projectName)
        {
            var projects = _projectsRepository.GetList()
                .Where(p => p.ProjectName.ToLower()
                    .Contains(projectName.ToLower()))
                .ToList();

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