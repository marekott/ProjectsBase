using System;
using System.Net;
using System.Web.Mvc;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;

namespace ProjectsBaseWebApplication.Controllers
{
    public class AuditTeamController : Controller
    {
        private readonly IRepository<AuditTeam> _auditTeamRepository;

        public AuditTeamController(IRepository<AuditTeam> auditTeamRepository)
        {
            _auditTeamRepository = auditTeamRepository;
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
                AuditorId = new Guid("fb772a3e-2eb9-e911-aa99-c83dd49b75c4") //TODO info brane z danych logowania użytkownika
            };

            _auditTeamRepository.Add(auditTeam);

            return RedirectToAction("Index", "Home");
        }
    }
}