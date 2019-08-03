using System.Web.Mvc;
using ProjectsBaseShared.Data;

namespace ProjectsBaseWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;
        private readonly ProjectsRepository _projectsRepository;
        private bool _disposed;

        public HomeController()
        {
            _context = new Context();
            _projectsRepository = new ProjectsRepository(_context);
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