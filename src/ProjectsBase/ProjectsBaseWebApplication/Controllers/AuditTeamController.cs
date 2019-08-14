using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;

namespace ProjectsBaseWebApplication.Controllers
{
    public class AuditTeamController : Controller
    {
        private readonly IRepository<AuditTeam> _auditTeamRepository;
        private readonly IRepository<Auditor> _auditorRepository;

        public AuditTeamController(IRepository<AuditTeam> auditTeamRepository, IRepository<Auditor> auditorRepository)
        {
            _auditTeamRepository = auditTeamRepository;
            _auditorRepository = auditorRepository;
        }
        public ActionResult Add(Guid? guid)
        {
            if (guid == null || guid == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var auditTeam = new AuditTeam()
            {
                ProjectId = (Guid)guid,
                AuditorId =  _auditorRepository.GetList().First().AuditorId //TODO info brane z danych logowania użytkownika
            };

            _auditTeamRepository.Add(auditTeam);

            TempData["Message"] = "Your application was saved!";

            return RedirectToAction("Index", "Home");
        }
    }
}