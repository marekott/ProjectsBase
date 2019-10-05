using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;

namespace ProjectsBaseWebApplication.Controllers
{
    public class AuditTeamController : Controller
    {
        private readonly IRepository<AuditTeam> _auditTeamRepository;

        public AuditTeamController(IRepository<AuditTeam> auditTeamRepository, IRepository<Auditor> auditorRepository)
        {
            _auditTeamRepository = auditTeamRepository;
        }
        public ActionResult Add(Guid? guid) //TODO nie można aplikować na swój projekt
        {
            if (guid == null || guid == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var auditTeam = new AuditTeam()
            {
                ProjectId = (Guid)guid,
                AuditorId = Guid.Parse(User.Identity.GetUserId())
            };

            _auditTeamRepository.Add(auditTeam);

            TempData["Message"] = "Your application was saved!";

            return RedirectToAction("Index", "Home");
        }
    }
}